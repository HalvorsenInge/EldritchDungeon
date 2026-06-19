---
domain: persistence-save
related: [character-creation, game-loop]
---

# Persistence & Save — Mental Model

## save-format

- OWNS: JSON serialization for player characters, world seed, and minimal world state
- DECIDED: JSON chosen for human-readability and tooling interoperability
- INVARIANT: every save includes schema_version and timestamp

## save-load

- OWNS: restore pipeline (validate → migrate if possible → hydrate objects → resume state)
- INVARIANT: loading must detect mismatched version/schema and fail-safe (preserve corrupted save as .broken)
- FLOW: open → validate header → migrate → hydrate → resume

## ironman-permadeath

- OWNS: permadeath semantics (no manual reload allowed for ironman runs)
- DECIDED: ironman mode disallows manual file-based reloading to enforce stakes
- INVARIANT: ironman saves are write-once per run unless debug mode enabled

## autosave-hooks

- OWNS: points in main-loop where autosave may trigger (end of floor, on death, manual save if allowed)
- DECIDED: autosave frequency set conservatively to avoid trivializing permadeath
- TENSION: frequent autosaves vs player expectation of meaningful stakes

## save-validation

- OWNS: integrity checks (schema version, checksums, seed consistency)
- FLOW: on load → validate schema → if mismatch: attempt migration → if migration fails: preserve and surface error

## backup-and-rollback

- OWNS: rotate a small number of backups (.bak) for recent saves to support crash recovery
- DECIDED: keep 3 backups per profile and purge oldest on new save

## save-encryption

- OWNS: optional obfuscation/encryption for ironman metadata to prevent tampering
- TENSION: user control vs anti-cheat posture; default disabled for single-player convenience
