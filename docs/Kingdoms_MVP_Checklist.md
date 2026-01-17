# Kingdoms — MVP Build Checklist

> Working copy for the repo (suggested path: `docs/MVP_Checklist.md`).  
> Use GitHub checkboxes: `- [ ]` (unchecked) / `- [x]` (checked).

## Phase 0 — Groundwork
- [x] Create Unity 6.2 URP project (`Kingdoms`) and repo with Unity `.gitignore` (Library/Temp/Builds ignored)
- [x] Add `Scenes/Smoke.unity` (simple text UI: “Kingdoms Smoke Test”)
- [x] Set Player defaults: windowed, 1280×720 (resizable), company/product names
- [x] Editor settings: **Force Text** serialization; **Visible Meta Files**
- [ ] Add issue/bug template in repo (expected/actual, steps, logs)

**Exit:** Fresh clone builds locally; Smoke scene runs at 60fps on a mid laptop.

---

## Phase 1 — Rules Engine (pure C#)
- [ ] 9×9 grid; castles fixed (A: 4,4; B: 6,6 from A’s bottom-left origin)
- [ ] Legal placement: empty tile adjacent (8-neighborhood) to any enemy piece **or** enemy castle
- [ ] Flips: Othello-style along ranks/files only; castle is neutral (never counts as a colored piece)
- [ ] Coins: +1 coin if turn completes and at least one tile occupied; on 3 coins → draw a card
- [ ] Hand: max 3; **must play** a card at start of turn if hand is full
- [ ] Shared deck: single shuffled deck; deterministic seed recorded
- [ ] Card timing: **card effect resolves first**, then standard placement; flips apply after each placement in that order
- [ ] Response timing: opponent decides to respond **before** seeing the move; response may cancel/steal/end turn per card
- [ ] Visibility: played cards visible during resolution; at end of turn go face-down to bottom of shared deck (card counting allowed)
- [ ] No-move handling: if no legal placement **and** no playable card, skip turn (no coin)
- [ ] Victory: castle “hold” requires N/S/E/W control until your next turn; fallback = most pieces when no moves remain for either
- [ ] Determinism: seed covers deck order, player start, any randomness
- [ ] API surface: `GameState`, `PlayerState`, `Deck`, `Card`, `TurnIntent`, `ResolvedTurn`, `ValidationResult`, `EventLog`

**Unit tests (green):**
- [ ] Flips (single/multi-line, blocked by empty, castle-neutral)
- [ ] Adjacency legality around enemy/castle
- [ ] Coin/hand rules including must-play when hand==3
- [ ] Deck push-to-bottom and visibility window
- [ ] Castle surround hold check across turns
- [ ] All **14 cards** incl. *Sabotage* and *Counter Offensive* timing

---

## Phase 2 — Simulation Harness (headless)
- [ ] CLI runner to apply scripted turns and save/load `GameState` snapshots
- [ ] Golden trace logs for determinism (committed)
- [ ] Scenario packs: each card, “no moves”, castle hold, response timing

**Exit:** One command replays a whole match identically from the same seed.

---

## Phase 3 — Greybox Board UI (local hot-seat)
- [ ] Render 9×9 grid, castles, pieces; legal-move highlights
- [ ] Hand UI (≤3), coin counter, deck count (optional)
- [ ] Show opponent coin count + “has response card” indicator
- [ ] Action log (basic text), **undo within turn** (before submit)
- [ ] On-screen rules sheet/help (text + simple diagrams)

**Exit:** Two humans can finish a match locally without external docs.

---

## Phase 4 — Turn & Card UX (hidden info flow)
- [ ] Card-play step (preview is hidden from opponent)
- [ ] Opponent sees: “Opponent played a card — respond?” (no board reveal)
- [ ] Resolution animates: **card effect → placement → flips**
- [ ] End-turn summary: flips gained, coins Δ, cards used

**Exit:** Testers report the flow is clear and matches rules.

---

## Phase 5 — Networking (server-authoritative async 1v1)
- [ ] Guest username entry (unique + optional discriminator)
- [ ] Challenge flow: search username → send invite → accept → create match
- [ ] Transport: UGS Relay (or chosen), server-author host runs Phase-1 engine
- [ ] Reconnect by match ID; mid-turn resumes; desync detector (state hash per resolved turn)
- [ ] Spectator mode: read-only (board + counts; no card names)

**Exit:** Two remote machines finish a match; reconnect works; spectate works.

---

## Phase 6 — Persistence & Lifecycle
- [ ] Snapshot per **resolved turn** (append-only)
- [ ] TTL: inactive matches auto-expire at **14 days**
- [ ] Cleanup: unfinished hard-deleted at **30 days**; finished retained 30 days
- [ ] “Your turn” share link (URL copy to clipboard with match+player token)

**Exit:** Close app → reopen elsewhere → continue the same match; old matches pruned.

---

## Phase 7 — Telemetry & Errors (lightweight)
- [ ] Counters: DAU, matches created/finished, avg turns/match, median move time, resigns, timeouts
- [ ] Card stats: plays per card, win % when played, steal/cancel frequencies
- [ ] Errors: client/server exceptions, desync hits, failed reconnects
- [ ] Opt-in anonymous analytics toggle

**Exit:** Quick glance answers: “Are people finishing? Which cards overperform? Any desyncs?”

---

## Phase 8 — Playtest-Ready Build
- [ ] Rules sheet screen + 3–5 illustrated examples (static is fine)
- [ ] Bug-report hotkey: copies last logs + state hash to clipboard
- [ ] Build stamp: commit + seed on main menu
- [ ] Zip/exe build (no installer) and short “How to invite & play” note

**Exit:** Anyone can download and play with minimal instruction.

---

## Phase 9 — Balance & Iteration
- [ ] Card definitions externalized (JSON/ScriptableObject)
- [ ] Safe-list of tunables (deck counts, params) that won’t break saves
- [ ] Monte Carlo bot sim (1,000+ runs) for outlier detection

**Exit:** You can tweak a card and regenerate a build in minutes.

---

## Phase 10 — Stretch (post-MVP)
- [ ] Friends list; quick rematch
- [ ] Tutorial/narration card; subtle animations; SFX hooks (FMOD)
- [ ] Mobile-friendly scaling (if desired)

---

### Notes / Decisions Log
- Shared deck; card visible during resolution; bottom-of-deck after turn
- Response decision before move is revealed
- Card effect resolves before standard placement; flips after each placement
- Matches expire at 14 days inactivity; deleted at 30 days
- Spectators see board + counts, not card names
