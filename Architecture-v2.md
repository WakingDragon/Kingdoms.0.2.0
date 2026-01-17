# Kingdoms – Gameplay Architecture

This document defines the core gameplay architecture for **Kingdoms**, with an eye toward:

* Clean separation between **engine**, **session**, **services**, and **Unity presentation**.
* Future **server-authoritative online play**, while still supporting **local hot-seat**.

The goal is that Copilot (and future collaborators) can understand:

* **Who does what**
* **Who depends on what**
* **How to extend this to online play**

---

## 1. Layer Overview

### Shared elements

`IGameEngine` exists on both client and server. The server engine is authoritative and is the only one allowed to apply actions to GameState. Client engines are advisory only (hints, previews, optimistic validation).
* Domain (BP.Kingdoms.Core)
* Rules Engine (BP.Kingdoms.Engine)

### Server-authoritative runtime (online)
* Match (BP.Kingdoms.Match): `IMatch`, `Match`
* Services (BP.Kingdoms.Services): `MatchService` (manages matches)

### Client-side runtime
* Client (BP.Kingdoms.Client): `IGameClient`, `LocalGameClient`, `RemoteGameClient`
* Shell (BP.Kingdoms.Shell): `GameShell` (start/join/rejoin orchestration)
* Presentation (BP.Kingdoms.UI.Presentation / .Views): Unity UI & tabletop
* Bootstrap (BP.Kingdoms.Bootstrap): Unity wiring & scene loading

---

## 2. Namespaces

Summary of the namespace structure and relationship between namespaces: from core (foundation level) to UI (top-level views). 

Purpose is to keep namespaces clean and tight.

### 2.1 `BP.Kingdoms.Core`

**Responsibility:** Core types used in the game

Put here (pure domain types, no rules logic):

* `GameState`, `BoardState`, `PlayerState`
* Enums/value objects: `PlayerId`, `TurnPhase`, `TileOccupant`, `Coord`
* “Data-only” models: cards, decks, resources, etc.
* `IGameAction` + concrete action types if they’re pure domain data (recommended)
* `RuleResult`, `RuleVerdict` (these are domain-level results)

**Dependencies:** Only allowed to reference `System`.x

**Cannot reference:** Unity or any other modules.

### 2.2 `BP.Kingdoms.Engine`

**Responsibility:** Authority for the rules

Put here (rules + evaluation + hints + preview):

* `IGameEngine`
* `GameEngine` implementation
* Any rule modules / evaluators / calculators
* Turn preview models (e.g. `TurnPreview`, `PreviewEvent`)
* Hint generation logic

**Dependencies:** `BP.Kingdoms.Core`

**Cannot reference:** Unity, Networking, etc 

Notes: Your current IRule and Reducer live here (or inside sub-namespaces like BP.Kingdoms.Engine.Rules / BP.Kingdoms.Engine.Reduction) but are internal details behind GameEngine.

### 2.3 `BP.Kingdoms.Match`

**Responsibility:** Authoritative 'one-game' runtime.

Put here:
* `IMatch` (renamed from IGameSession)
* `Match` (renamed from GameSession)
* Match events/types: `MatchId`, `MatchEvent`, `ActionReceipt`, etc.
* Match-level policies that are not “rules”: e.g. “whose turn is it?”, “can this player act now?”, (later) time controls, resign, draw offer

**Dependencies:** `BP.Kingdoms.Core`, `BP.Kingdoms.Engine` (it uses the engine)

**Cannot reference:** Unity, Presentation, Concrete networking transports (see below)

### 2.4 `BP.Kingdoms.Services`

**Responsibility:** Match manager and host/server facade. Put here:

* `MatchService` (renamed from GameService)
* “create/join/find match” orchestration
* Match storage interface and implementations: `IMatchRepository` (in-memory now, DB later)
* (later) authentication mapping: external user id → `PlayerId`
* (later) persistence hooks: save state snapshot, append event log

**Dependencies:** `BP.Kingdoms.Match` (manages matches), `BP.Kingdoms.Engine` (to construct matches), `BP.Kingdoms.Core`

**Cannot reference:** Unity / presentation, Concrete network transports (keep those in a separate namespace below, Services raises events to which .Net subscribes)

### 2.5 `BP.Kingdoms.Client`

**Responsibility:** A client-side façade used by the UI to interact with a match, either locally (wrapping an in-process `IMatch`) or remotely (network calls to a server). Provides state updates, hint/preview requests, and action submission.

Put here:
* `IGameClient` (renamed from `IClientGameSession`)
* `LocalGameClient` (wraps a local `IMatch`)
* `RemoteGameClient` (future: talks to a server)
* Client-side DTO mapping (optional): action/state serialization helpers

**Dependencies:** `BP.Kingdoms.Core` (state/actions), May depend on `BP.Kingdoms.Engine` for client-side hints/previews without server roundtrip (that’s optional; you can still keep engine on client but avoid “authoritative apply”). Local mode depends on `BP.Kingdoms.Match` (because it wraps a match)

**Cannot reference:** Unity presentation types (keep those separate), Server hosting code (no `MatchService` unless in local dev composition root)

### 2.6 `BP.Kingdoms.Shell`

**Responsibility:** orchestrates game start/end

Put here:
* `IGameShell`, `GameShell`
* “Start new game / Join match / Rejoin / Leave” orchestration
* Choosing which client to use: local vs remote
* Scene transitions / lifecycle hooks (but not actual rendering)

**Dependencies:** `BP.Kingdoms.Client`, `BP.Kingdoms.Core`

In Unity, it may reference Unity scene-loading APIs if you want — but ideally keep Unity calls in `BP.Kingdoms.Unity` and have shell as plain C#.

### 2.7 `BP.Kingdoms.UI`

**Responsibility:** Translates player input to action

Contains 3 sub-namespaces:

#### 2.7.1 `BP.Kingdoms.UI.Contracts`

**Responsibility:** This will hold interfaces for decoupling the larger 'full page' UI elements. For now it is just a placeholder.

#### 2.7.2 `BP.Kingdoms.UI.Views`

**Responsibility:** visual display elements, raise events that are read by Presenters

Put here
* `BoardView`, `CellView`, `HandsView`
* `MainMenuView`, etc
* Theme assets like `BoardThemeAsset`

#### 2.7.3 `BP.Kingdoms.UI.Presenters`

**Responsibility:** issues commands to Views and receives back messages from Views (input) which it sends to the Client

Put here:
* `BoardPresenter`, `HandsPresenter`
* `MainMenuPresenter`, `SettingsMenuPresenter`
* Other menus like lobby, game history, etc when created
* UI glue that converts clicks → `IGameAction` or `TurnIntentDraft`

**Dependencies:** `BP.Kingdoms.Client` (talks to `IGameClient`), `BP.Kingdoms.Core` (reads `GameState`, creates actions), .`Views`/.`Contracts`

### 2.8 `BP.Kingdoms.Net` (online transport layer)

**Responsibility:** Subscribes to .Services to send and receive data

Net may have Server and Client sub-namespaces; the client side contains transport adapters used by RemoteGameClient.

Put here:
* WebSocket/HTTP handlers, message codecs
* DTOs: ActionDto, StateDto, HintRequestDto, etc.
* Server endpoints/routers
* Client transport adapters used by RemoteGameClient

**Dependencies:** `BP.Kingdoms.Services` (server routes call service), `BP.Kingdoms.Core` (DTO mapping), On the client, RemoteGameClient needs a transport adapter.

**Cannot reference:** Must not depend on Unity

### 2.9 `BP.Kingdoms.Bootstrap`

**Responsibility:** Unity-specific composition root and lifecycle glue: creates/wires `GameShell`, chooses local vs remote client implementations, performs scene loading.

Put here:
* `KingdomsBootstrapper`
* Scene controls
* Wiring code

**Dependencies:** `BP.Kingdoms.Shell` (which actually does the game starting etc.)
* `BP.Kingdoms.Shell`
* `BP.Kingdoms.Client`
* `BP.Kingdoms.Engine`
* (Local mode) `BP.Kingdoms.Match`
* (Online) `BP.Kingdoms.Net (composition + adapters)`

---

## 3. Core Domain & Rules Engine Boundary

This section defines where the line is drawn between:
* Pure domain state (`BP.Kingdoms.Core`)
* Rules authority (`BP.Kingdoms.Engine`)

Nothing in this section depends on Unity, networking, or services.

### 3.1 Core Domain (`BP.Kingdoms.Core`)

`GameState` and domain types

#### Responsibility
* Represents a complete snapshot of a match at a single moment in time
* Is passive data, not behavior
* Is safe to:
  * Serialize
  * Diff
  * Hash
  * Replay deterministically

#### Contains
* `BoardState`
* `Dictionary<PlayerId, PlayerState>`
* `PlayerId` CurrentPlayerId
* `TurnPhase`
* Deck/hand/resource state as needed

#### Allowed helpers (STRICTLY LIMITED)

Helpers may:
* Move pointers or flags without making decisions
* Enforce no rules

Examples of allowed helpers:
* AdvancePhase();
* SetCurrentPlayer(PlayerId next);

Examples of forbidden helpers:
* CanPlaceAt(...)
* ApplyPlacement(...)
* ResolveFlips(...)

Rule of thumb:
If a method needs to think, it belongs in BP.Kingdoms.Engine.

#### Dependencies
* Other Core types only (`BoardState`, `PlayerState`, enums, value objects)

#### Interfaces
* None — domain objects are concrete and serializable.

#### Mapping
* Already correctly located at `BP.Kingdoms.Core.GameState`

### 3.2 Player Intent (`IGameAction`)
`IGameAction` and concrete actions

#### Responsibility

Represent player intent, not outcome

Must be:
* Serializable
* Deterministic
* Network-safe
* Examples:
  * PlacePieceAction
  * PlayCardAction
  * EndTurnAction

**Important distinction**

An `IGameAction`:
* Does not know whether it is legal
* Does not apply itself
* May be rejected by the engine

#### Dependencies
* Core value types only (PlayerId, Coord, card ids, etc.)

#### Interfaces
```
public interface IGameAction
{
    PlayerId Actor { get; }
}
```

#### Mapping
* Lives in `BP.Kingdoms.Core`
* Travels UI → Client → Match → Engine

### 3.3 Rule Evaluation Results (`RuleResult`, `RuleVerdict`)

#### Responsibility
* Represent the outcome of engine evaluation
* Used for:
  * UI feedback
  * Logging
  * Server rejection reasons

#### Examples
* `Success`
* `Invalid`
* `IllegalMove`
* `OutOfTurn`
* `RuleViolation("Must play a card when hand is full")`

#### Dependencies
* Core primitives only (enums, strings)

#### Mapping
* Lives in `BP.Kingdoms.Core`
* Returned by `IGameEngine`

### 3.4 Rules Authority (`BP.Kingdoms.Engine`)
`IGameEngine`

#### Responsibility
* The only place where rules are interpreted
* The only place allowed to:
  * Validate actions
  * Apply actions to `GameState`
* Exists on:
  * Server (authoritative)
  * Client (advisory only: previews, hints)

#### Conceptual interface
```
public interface IGameEngine
{
    RuleResult Validate(GameState state, IGameAction action);
    void Apply(GameState state, IGameAction action);

    // Optional, non-authoritative helpers
    IReadOnlyList<Coord> GetHints(GameState state, PlayerId player);
    // TurnPreview Preview(GameState state, IGameAction action);
}
```

#### Critical guarantees
* `Apply` is only called by `Match` (server and local); never by UI/services/net/client directly.
* UI, Services, and Networking never bypass this interface

#### Internal structure (hidden)
Inside the engine you may use:
* Rules
* Reducers
* Pipelines
* State machines

These are **implementation details**, not architectural dependencies.

#### Mapping
* Existing `IRule` / `Reducer` become internal engine details
* External layers depend only on `IGameEngine`

---

## 4 `Match`

### 4.1 `Match`

#### Responsibility
* Represents one running match in memory.
* Owns:
  * A single `GameState`
  * A unique `MatchId`
* Enforces match-level policies (not engine rules), e.g.:
  * “Is it this player’s turn?”
  * “Is the match finished?”
  * (later) time controls, resign/draw offers
* Delegates rules enforcement to `IGameEngine`:
  * `Validate` actions
  * `Apply` actions
* Produces domain events / receipts for observers (services, network, logging, client).

#### What it does NOT do
* Does not render
* Does not reference Unity
* Does not push “hints” as a core feature
* Does not know about transports / sockets / HTTP

#### Key dependencies
* From `.Core`:
  * `GameState`, `IGameAction`
  * `PlayerId`, `Coord`
  * `RuleResult` / `RuleVerdict`
* From `.Engine`:
  * `IGameEngine`
* `Match` types (in `.Match`):
  * `MatchId`
  * `ActionReceipt`
  * `MatchEvent`

#### Interfaces

`IMatch` — interface over `Match`:
* Properties:
  * `MatchId Id { get; }`
  * `GameState State { get; }`
  * `bool IsFinished { get; }` (or `MatchStatus Status`)
* Methods:
  * `ActionReceipt Submit(IGameAction action)`
  * Optional: `IReadOnlyList<MatchEvent> DrainEvents()` (or `IEnumerable<MatchEvent> GetPendingEvents()`)
  * Optional event hook (simple composition):
    * `event Action<MatchEvent> OnEvent`

#### Action submission flow
1. `Match` checks match-level policy:
  * correct actor?
  * match not ended?
2. `IGameEngine.Validate(State, action)`
3. If valid: `IGameEngine.Apply(State, action)`
4. Match emits:
  * `MatchEvent.ActionApplied(...)`
  * `MatchEvent.StateChanged(...)` (or more granular turn/phase events)
5. Returns `ActionReceipt` containing:
  * verdict
  * optional reason
  * optional state version / hash (later)

#### Suggested types
```
public readonly record struct MatchId(Guid Value);

public sealed record ActionReceipt(
    MatchId MatchId,
    IGameAction Action,
    RuleResult Result,
    int StateVersion // optional now, useful later
);

public abstract record MatchEvent
{
    public sealed record ActionApplied(IGameAction Action) : MatchEvent;
    public sealed record ActionRejected(IGameAction Action, RuleResult Reason) : MatchEvent;
    public sealed record MatchEnded(PlayerId? Winner) : MatchEvent;
}
```
(Exact shapes are flexible; the point is domain events, not UI callbacks.)

---

## 5. `MatchService` (Server / Host Layer)

### 5.1 `MatchService`

#### Responsibility

`MatchService` is the host-side manager of matches. It owns many `Match` instances and acts as the façade that networking layers talk to.

It is the only layer allowed to create, find, and destroy matches.

**Responsibilities include:**
* Creating new matches:
  * Assign `MatchId`
  * Seed initial `GameState`
  * Construct `Match` with `IGameEngine`
* Looking up matches by `MatchId`
* Routing incoming player actions to the correct `Match`
* Subscribing to `Match` events for:
  * Persistence
  * Network broadcasting
  * Logging / telemetry
* Lifecycle management:
  * Expiry
  * Cleanup
  * (Later) persistence & reload

**What it does NOT do**
* Does not interpret rules
* Does not mutate `GameState` directly
* Does not know about Unity
* Does not implement networking protocols

#### Key dependencies
* `BP.Kingdoms.Match`
  * `IMatch`, `Match`
  * `MatchId`
  * `MatchEvent`
* `BP.Kingdoms.Engine`
  * `IGameEngine`
* `BP.Kingdoms.Core`
  * `GameState`, `IGameAction`, `PlayerId`

### 5.2 Suggested interface
```
public interface IMatchService
{
    MatchId CreateMatch(MatchConfig config);
    IMatch GetMatch(MatchId id);
    bool TryGetMatch(MatchId id, out IMatch match);
}
```

**Notes**
* `MatchService` is long-lived (application lifetime)
* `Match` instances are short-lived (per game)
* In local hot-seat mode, you may bypass this layer and create a Match directly in the composition root

### 5.3 `Match` persistence (future)

Later, `MatchService` becomes the natural home for:
* `IMatchRepository`
  * In-memory (now)
  * Database-backed (future)
* Snapshotting `GameState`
* Appending event logs
* Rehydrating matches after reconnect

---

## 6. Client Layer (`BP.Kingdoms.Client`)

### 6.1 `IGameClient`

#### Responsibility
* `IGameClient` is the only API the UI talks to.
* It represents “my view of the current match”, regardless of whether the match is:
  * Local (hot-seat / dev mode), or
  * Remote (online, server-authoritative)
* It provides:
  * Read-only access to the latest known `GameState`
  * Methods to:
    * Submit actions
    * Request hints / previews
  * Events for:
    * State updates
    * Action results
    * Hints / previews

### 6.2 Conceptual interface
```
public interface IGameClient
{
    GameState State { get; }

    // Action submission
    void SubmitAction(IGameAction action);

    // Draft-based exploration
    void RequestHints(TurnIntentDraft draft);
    void RequestPreview(TurnIntentDraft draft);

    // Events
    event Action<GameState> OnStateChanged;
    event Action<IReadOnlyList<Coord>> OnHintsChanged;
    event Action<TurnPreview> OnPreviewChanged;
    event Action<ActionReceipt> OnActionResult;
}
```

### 6.3 `LocalGameClient`

#### Responsibility
* Wraps an in-process `IMatch`.

#### Used for:
* Hot-seat
* Single-player dev
* Early testing

#### Behaviour
* `SubmitAction` → calls `Match.Submit` and raises `OnActionResult` from the returned `ActionReceipt`
* Draft requests → forwarded directly to `Match`
* Subscribes to `Match` events and translates them into client events

#### Dependencies
* `BP.Kingdoms.Match`
* `BP.Kingdoms.Core`

### 6.4 `RemoteGameClient` (future)

#### Responsibility
* Client-side proxy for a server-hosted match.

#### Behaviour
* Holds:
  * `MatchId`
  * Transport adapter (WebSocket / HTTP)
* `SubmitAction`:
  * `SubmitAction` serialises and sends action to server.
  * When the server responds (or pushes), the client raises:
    * `OnActionResult`(receipt)
    * `OnStateChanged`(state) (if changed)
* Receives pushed updates:
  * State changes
  * Action receipts
  * Hint / preview results

**Important guarantee** 

From the UI’s perspective, `LocalGameClient` and `RemoteGameClient` behave identically.

---

## 7. Game Shell (`BP.Kingdoms.Shell`)

### 7.1 `GameShell`

#### Responsibility
* Top-level game lifecycle coordinator.
* Owns at most one active `IGameClient`.
* Handles:
  * Starting a new local match
  * Starting / joining an online match
  * Rejoining an existing match
  * Leaving / ending a match
  * Transitioning between menu and game scenes
* The UI never directly constructs matches or clients — it asks the shell.

### 7.2 Suggested interface
```
public interface IGameShell
{
    void StartLocalHotseatGame();
    void StartOnlineMatch();
    void JoinMatch(MatchId matchId);
    void EndCurrentMatch();

    IGameClient? ActiveClient { get; }
}
```

### 7.3 Unity integration
* Main menu UI talks only to `IGameShell`
* When a match starts:
  * Shell creates the appropriate `IGameClient`
  * Loads the board scene
  * Injects `IGameClient` into presenters
* When a match ends:
  * Shell disposes client
  * Returns to menu

---

## 8. Presentation Layer (`BP.Kingdoms.UI`)

### 8.1 Presenters

#### Responsibility
* Translate between:
  * Player input
  * `IGameClient` calls
  * View updates

* Presenters:
  * Own `TurnIntentDraft`
  * Convert UI interaction → draft updates
  * Request hints / previews
  * Convert a final draft → `IGameAction`
  * Submit action via `IGameClient`

#### Key rules

Presenters never:
* Mutate `GameState`
* Interpret rules
* Talk directly to Match or MatchService

### 8.2 Example: `BoardPresenter`

#### Responsibilities:
* Subscribe to:
  * `OnStateChanged`
  * `OnHintsChanged`
  * `OnPreviewChanged`
* Drive:
  * Board highlights
  * Animations
  * Invalid move feedback
* Convert:
  * Clicks / hovers → `TurnIntentDraft`
  * Confirm → `IGameAction`

### 8.3 Views

Views are purely visual:
* Render board, pieces, hands
* Raise UI events (clicks, hovers)
* Contain no game logic

They are ignorant of:
* Matches
* Clients
* Engines
* Rules

---

## 9. Networking Layer (`BP.Kingdoms.Net`) – Online Play

#### Responsibility
* Transport only.
  * Serialisation
  * Deserialisation
  * Message routing
* Server side:
  * Endpoints receive DTOs
  * Call into `MatchService`
  * Subscribe to `Match` events and broadcast updates
* Client side:
  * Transport adapters used by `RemoteGameClient`

#### Key rule

Networking depends on Services and Core — never the other way around.

---

## 10. Bootstrap (`BP.Kingdoms.Bootstrap`)

#### Responsibility

Unity-specific composition root.

Creates and wires:
* `IGameEngine`
* `MatchService` (server / local)
* `GameShell`
* Scene loading hooks

This is the only place where concrete implementations are chosen.

---

## 11. Final Mental Model
* Core — data only
* Engine — rules authority
* Match — one game, in memory
* MatchService — many matches
* GameClient — UI-facing façade
* GameShell — lifecycle orchestration
* UI — input + rendering
* Net — transport only

If a class does not clearly belong to one of these layers, it probably does not belong in the architecture yet.