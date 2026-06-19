---
domain: game-loop
related: [map-generation, persistence-save]
---

# Game Loop — Mental Model

## main-loop

- OWNS: top-level run loop and lifecycle (init, tick, shutdown)
- READS FROM: input-processing, event-queue
- WRITES TO: entity states, UI buffer, persistence autosave
- INVARIANT: only one main loop instance runs per process
- FLOW: start → load resources → enter tick loop → process input → update → render → resolve events → autosave/checks → repeat

## input-processing

- OWNS: translating raw player keystrokes into actions
- READS FROM: console input buffer
- WRITES TO: event-queue, command objects
- INVARIANT: input handlers are pure translators (no world mutation)

## update-phase

- OWNS: applying game rules, resolving actions, AI decisions
- READS FROM: event-queue, entity components, map state
- WRITES TO: entity components, turn-order, sanity/hp/mana
- TENSION: deterministic order vs performance for large numbers of entities

## render-phase

- OWNS: composing ASCII screen buffer (80x25)
- READS FROM: entity display components, map tiles
- WRITES TO: console output only (no game state mutation)
- DECIDED: ASCII-only renderer chosen for portability and design constraints

## turn-order

- OWNS: resolving action priority and initiative
- INVARIANT: turn resolution is stable within a tick (no reordering after start)
- FLOW: gather actions → sort by initiative/speed → execute sequentially → apply consequences

## event-queue

- OWNS: transient events between subsystems (deferred effects, triggers)
- READS FROM: input-processing, update-phase, AI
- WRITES TO: update-phase consumers, persistence hooks
- INVARIANT: events carry enough context to be replayable for save/load
