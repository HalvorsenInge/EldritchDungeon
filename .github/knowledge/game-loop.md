---
domain: game-loop
related: [map-generation, persistence-save]
---

# Game Loop — Mental Model

## main-loop

- OWNS: top-level run loop and lifecycle (init, tick, shutdown)
- OWNS: tick cadence policy (player-driven turn vs fixed tick)
- READS FROM: input-processing, event-queue, config (tick_rate, autosave)
- WRITES TO: entity states, UI buffer, persistence autosave, logs
- INVARIANT: only one main loop instance runs per process
- INVARIANT: each tick fully resolves before next tick begins
- FLOW: start → load resources → enter tick loop → process input → update → render → resolve events → autosave/checks → repeat
- TENSION: responsiveness (fast tick) vs determinism and CPU cost
- DECIDED: game is primarily turn-based; ticks advance on player action or timed waits for AI

## input-processing

- OWNS: translating raw player keystrokes into canonical actions/commands
- READS FROM: console input buffer, keybinds config
- WRITES TO: event-queue, command objects, input logs
- INVARIANT: input handlers are pure translators (no direct world mutation)
- TENSION: local key repeat handling vs explicit command queues for reproducibility

## update-phase

- OWNS: applying game rules, resolving actions, AI decisions, status effects
- READS FROM: event-queue, entity components, map state, RNG seed
- WRITES TO: entity components, turn-order, sanity/hp/mana, action logs
- INVARIANT: deterministic outcome given same seed and inputs
- TENSION: batching many entity updates for performance vs strict sequential predictability
- FLOW: collect intents → resolve by initiative → apply effects → enqueue follow-up events

## render-phase

- OWNS: composing ASCII screen buffer (80x25) and debug overlays
- READS FROM: entity display components, map tiles, message log
- WRITES TO: console output only (no game state mutation)
- DECIDED: ASCII-only renderer chosen for portability and design constraints
- TENSION: renderer must be fast and side-effect free to allow replay and testing

## turn-order

- OWNS: resolving action priority and initiative, handling interrupts
- INVARIANT: turn resolution is stable within a tick (no reordering after start)
- FLOW: gather actions → sort by initiative/speed → execute sequentially → apply consequences → resolve chained events
- TENSION: simultaneous actions (AoE) need atomic resolution vs visible interleaving

## event-queue

- OWNS: transient events between subsystems (deferred effects, triggers, replayable logs)
- READS FROM: input-processing, update-phase, AI, scripted triggers
- WRITES TO: update-phase consumers, persistence hooks, message log
- INVARIANT: events carry enough context to be replayable for save/load
- DECIDED: events are serializable for debugging and save replay support
