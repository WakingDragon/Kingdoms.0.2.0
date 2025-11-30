# Kingdoms – Gameplay Architecture

This document defines the core gameplay architecture for **Kingdoms**, with an eye toward:

* A clear, central **`IGameEngine`** that owns the rules.
* Clean separation between **engine**, **session**, **services**, and **Unity presentation**.
* Future **server-authoritative online play**, while still supporting **local hot-seat**.

The goal is that Copilot (and future collaborators) can understand:

* **Who does what**
* **Who depends on what**
* **How to extend this to online play**

---

## 1. Layer Overview

From “closest to the rules” to “closest to the player”:

1. **Domain / Rules Engine Layer**

   * `GameState`, `BoardState`, `PlayerState`, `TurnPhase`
   * `IGameAction`
   * `RuleResult`, `RuleVerdict`
   * `IGameEngine` (new)
   * Internal helpers (e.g. `IRule`, `Reducer`) – hidden behind `IGameEngine`

2. **Session Layer**

   * `GameSession` (one running match)
   * `IGameSession` (interface over `GameSession`)

3. **Service Layer**

   * `GameService` (match manager – creates/fetches `GameSession`s; mostly for server/host)

4. **Client Session Abstraction**

   * `IClientGameSession`
   * `LocalClientGameSession` (wraps a local `GameSession`)
   * `RemoteClientGameSession` (future – talks to server)

5. **Presentation Layer (Unity)**

   * `BoardPresenter`, `HandsView`, `BoardView`, etc.

---

## 2. Domain / Rules Engine Layer

### 2.1 `GameState` and domain types

**Responsibility**

* Represents a **snapshot of an entire match at a single moment**.
* Contains:

  * `BoardState`
  * `Dictionary<PlayerId, PlayerState>`
  * `PlayerId CurrentPlayerId`
  * `TurnPhase` (Start, CardEffect, Placement, Flip, End)
* Offers simple helpers like `BeginTurn`, `AdvancePhase`, `EndTurn`.

**Key dependencies**

* `BoardState`, `PlayerState`
* `PlayerId`, `TurnPhase`, `TileOccupant`

**Interfaces**

* None – `GameState` is a **concrete data class**.

**Current mapping**

* Already exists as `BP.Kingdoms.Core.GameState`.

---

### 2.2 `IGameAction` and concrete actions

**Responsibility**

* Represent **“what the player is trying to do”**:

  * e.g. “place a piece at (x,y)”, “play card #3”, “fortify tile”.
* This is the unit that travels:

  * from UI → client session
  * from client → server
  * from server → engine (`IGameEngine`).

**Key dependencies**

* `PlayerId`
* Value types like `Coord` / `Vector2Int`.

**Interfaces**

* `IGameAction`

  * Implemented by concrete actions (e.g. `PlacePieceAction`, `PlayCardAction`).

**Current mapping**

* You already have `IGameAction` in your rules/core code; continue to use it.

---

### 2.3 `RuleResult` / `RuleVerdict`

**Responsibility**

* Encapsulate the result of validating an action:

  * `Success`, `Invalid`, `IllegalMove`, `Error` etc.
* Carries an optional message for UI / logging.

**Key dependencies**

* Basic enums / strings only.

**Interfaces**

* None, just types used by `IGameEngine` and above layers.

---

### 2.4 `IGameEngine` (central rules facade)

**Responsibility**

* The **single entry point** for the game rules.
* Provides operations over `GameState` and `IGameAction`:

  * Validate actions.
  * Apply actions to produce new state (or mutate in-place for now).
  * Generate hints / previews.

Conceptual interface:

```csharp
public interface IGameEngine
{
    RuleResult Validate(GameState state, IGameAction action);
    void Apply(GameState state, IGameAction action);

    // Optional helper APIs:
    List<Coord> GetHints(GameState state, PlayerId playerId);
    // TurnPreview GetPreview(GameState state, IGameAction action);
}
```

Everything outside the rules layer depends on **`IGameEngine`**, not `IRule`/`Reducer` directly.

**Key dependencies**

* `GameState`
* `IGameAction`
* `RuleResult`, `Coord`, `PlayerId`
* Internally: whatever implementation you like (`IRule`, `Reducer`, random, etc.)

**Interfaces**

* `IGameEngine` – the core rules interface.

**Current mapping**

* You currently have `IRule` + `Reducer` doing this job.
* Implementation plan:

  * Implement `IGameEngine` as a thin wrapper that uses your existing `IRule` and `Reducer` internally.
  * Everyone else (sessions, services, client) talks only to `IGameEngine`.

---

## 3. Session Layer

### 3.1 `GameSession`

**Responsibility**

* Represents **one running match in memory**.
* Owns a single `GameState` for that match.
* Uses `IGameEngine` to:

  * Validate `IGameAction`s (`Validate`).
  * Apply valid actions (`Apply`).
  * Generate hints (`GetHints`).
* Raises events for observers (UI, network, logging):

  * When hints are available / updated.
  * When an action is applied and the state changes.

**Key dependencies**

* `GameState`
* `IGameEngine`
* `IGameAction`
* `RuleResult`
* `Coord`, `PlayerId`
* Event types:

  * `Action<List<Coord>> OnHints`
  * `Action<IGameAction, GameState, RuleResult> OnActionProcessed`

    * or `OnApplied(IGameAction, GameState)` + a separate error pathway.

**Interfaces**

* `IGameSession` – interface over `GameSession`:

  * `Guid MatchId { get; }`
  * `GameState State { get; }`
  * Events:

    * `OnHints(List<Coord> hints)`
    * `OnActionProcessed(IGameAction action, GameState state, RuleResult result)`
  * Methods:

    * `void PushHints(PlayerId player)`
    * `RuleResult TryApply(IGameAction action)`

**Current mapping**

* Your existing `GameService` in `BP.Kingdoms.Core` is effectively this `GameSession`:

  * It owns `gameState`.
  * It has `IRule` and uses `Reducer`.
  * It exposes `OnHints` and `OnApplied`.

Refactor target:

* `GameService` → `GameSession`, using an `IGameEngine` instead of raw `IRule`/`Reducer`.
* Implement `IGameSession` over it.

---

## 4. Service Layer

### 4.1 `GameService` (match manager / host)

**Responsibility**

* **Server/host-side manager** of multiple `GameSession`s:

  * Create new matches (e.g. `CreateNewSession(seed, config)`).
  * Look up matches by `MatchId`.
  * Route incoming actions from network into the correct `GameSession`.
* Natural place for:

  * Persistence (saving/loading match state).
  * Cleanup / expiry of sessions.
  * Mapping API users / auth → `PlayerId`s.

**Key dependencies**

* `IGameEngine` – injected once and used to build sessions.
* `IGameSession` / `GameSession`.
* `Guid` – match identifiers.
* Optional: persistence / logging services.

**Interfaces**

* You *can* add `IGameService` if needed:

  * `Guid CreateNewSession(...)`
  * `IGameSession GetSession(Guid matchId)`
  * etc.

**Current mapping**

* This does **not** exist yet.
* For now, local hot-seat can use a single `GameSession` directly.
* Later, server code will use `GameService` to manage multiple matches.

---

## 5. Client Session Abstraction

### 5.1 `IClientGameSession`

**Responsibility**

* Define what the **Unity client** needs from “the current match”, without caring whether it’s local or remote.
* Provide:

  * Read-only access to the last known `GameState`.
  * Events:

    * `OnStateChanged(GameState state)`
    * `OnHintsChanged(List<Coord> hints)`
    * `OnActionResult(IGameAction action, RuleResult result)`
  * Methods:

    * `RequestHints(PlayerId player)`
    * `SubmitAction(IGameAction action)`

**Key dependencies**

* `GameState`
* `IGameAction`
* `RuleResult`
* `Coord`, `PlayerId`

**Interfaces**

* `IClientGameSession` (client-facing abstraction).

**Current mapping**

* Not yet implemented; will live in a client-facing namespace (e.g. `BP.Kingdoms.Client`).

---

### 5.2 `LocalClientGameSession` (dev / hot-seat)

**Responsibility**

* Implement `IClientGameSession` by wrapping a **local `IGameSession`** (in-process).
* For development and hot-seat mode:

  * `SubmitAction` → call `_session.TryApply(action)`.
  * `RequestHints` → call `_session.PushHints(player)`.
* Translate session events into client events:

  * Session “action processed” → `OnStateChanged` + `OnActionResult`.
  * Session hints → `OnHintsChanged`.

**Key dependencies**

* `IGameSession` / `GameSession`
* `GameState`, `IGameAction`, `RuleResult`, `Coord`

**Interfaces**

* Implements `IClientGameSession`.

**Current mapping**

* Does not exist yet.
* In the current code, `BoardPresenter` talks directly to `GameService`; after refactor it will talk to a `LocalClientGameSession`.

---

### 5.3 `RemoteClientGameSession` (future online mode)

**Responsibility**

* Implement `IClientGameSession`, but talk to a **remote server** instead of local `GameSession`.
* Holds:

  * `Guid MatchId` (which server-side `GameSession` it’s bound to).
* Behaviour:

  * `SubmitAction` → serialise action + `MatchId`, send to server (HTTP/WebSocket/etc.).
  * `RequestHints` → send hint request to server.
  * Receive state updates / action results from server:

    * Update local `State`.
    * Raise `OnStateChanged` / `OnActionResult`.
  * Receive hints from server:

    * Raise `OnHintsChanged`.

**Key dependencies**

* Networking stack (HTTP/WebSocket).
* `GameState`, `IGameAction`, `RuleResult`, `Coord`, `PlayerId`.
* `Guid` / match IDs.

**Interfaces**

* Implements `IClientGameSession`.

**Current mapping**

* Planned future work for online play.

---

## 6. Presentation Layer (Unity)

### 6.1 `BoardPresenter`

**Responsibility**

* Unity-side **glue** between the client session and visual components:

  * Subscribes to `IClientGameSession`:

    * `OnStateChanged` → repaint/animate board & hands based on `GameState`.
    * `OnHintsChanged` → highlight legal cells on the board.
    * `OnActionResult` → show feedback for invalid moves.
  * Converts user input into `IGameAction`s:

    * Cell clicked → e.g. `PlacePieceAction`.
    * Future: card selected + cell clicked → combined action.

**Key dependencies**

* `IClientGameSession`
* `BoardView`, `HandsView`, `BoardThemeAsset`
* `GameState`, `Coord`, `PlayerId`, `TileOccupant`
* Unity types (`MonoBehaviour`, `Vector2Int`, `Color`)

**Interfaces**

* None – it’s a concrete MonoBehaviour.
* Crucially depends on **`IClientGameSession`** rather than a concrete session implementation.

**Current mapping**

* Exists as `BP.Kingdoms.Presentation.BoardPresenter`.
* Currently wired directly to `GameService`.
* Refactor target:

  * `Init(GameService service, PlayerId thisPlayer)` → `Init(IClientGameSession session, PlayerId thisPlayer)`.
  * Use `session.State` instead of `service.gameState`.
  * Subscribe to `session.OnStateChanged`/`OnHintsChanged`/`OnActionResult`.

---

### 6.2 `BoardView` / `HandsView` / etc.

**Responsibility**

* Purely **visual responsibilities**:

  * `BoardView`: build grid, return `CellView`s, show pieces/hints/castles.
  * `HandsView`: show cards/coins, communicate clicks back to `BoardPresenter`.

**Key dependencies**

* Unity UI components.
* `GameState` or simple view models derived from it.
* `BoardThemeAsset` for sprites/colours.

**Interfaces**

* None – concrete view components, kept ignorant of rules/session concepts.

---

## 7. Current → Target Mapping Summary

For Copilot and human sanity, here’s how existing classes map into this architecture:

* `BP.Kingdoms.Core.GameState`

  * **Remains:** `GameState` in the Domain/Rules Engine layer.

* `IRule` + `Reducer`

  * **Remain**, but **internal** to the rules engine.
  * Implement a new **`IGameEngine`** that wraps them so everything else talks to `IGameEngine`.

* `BP.Kingdoms.Core.GameService`

  * **Becomes:** `GameSession` in the Session layer.
  * Implements `IGameSession`.
  * Depends on `IGameEngine` instead of directly on `IRule` / `Reducer`.

* New types to introduce:

  * `IGameEngine`
  * `IGameSession`
  * `GameService` (match manager, server/host-side)
  * `IClientGameSession`
  * `LocalClientGameSession`
  * `RemoteClientGameSession` (future online)

* `BoardPresenter`

  * Refactor to depend on `IClientGameSession` rather than `GameService / GameSession`.
  * All board/hands updates happen in response to client-session events and `State`.

With this model:

* The **core rules** live behind `IGameEngine`.
* A **`GameSession`** is one match using that engine.
* A **host/server `GameService`** manages many sessions.
* The **Unity client** only sees `IClientGameSession`, which can be:

  * A `LocalClientGameSession` wrapping a local `GameSession` (today).
  * A `RemoteClientGameSession` talking to a server (future online).


