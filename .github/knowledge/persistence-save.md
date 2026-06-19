---
domain: persistence-save
related: [character-creation, game-loop]
---

# Persistence & Save — Mental Model

## save-format

- OWNS: JSON serialization for player characters, world seed, and minimal world state
- DECIDED: JSON chosen for human-readability and tooling interoperability

## save-load

- OWNS: restore pipeline (validate → hydrate objects → resume state)
- INVARIANT: loading must detect mismatched version/schema and fail-safe

## ironman-permadeath

- OWNS: permadeath semantics (no manual reload allowed for ironman runs)
- DECIDED: ironman mode disallows manual file-based reloading to enforce stakes

## autosave-hooks

- OWNS: points in main-loop where autosave may trigger (end of floor, on death)
- TENSION: autosave frequency vs player control and expected challenge

## save-validation

- OWNS: integrity checks (schema version, checksums, seed consistency)
- FLOW: on load → validate schema → if mismatch: attempt migration else error
