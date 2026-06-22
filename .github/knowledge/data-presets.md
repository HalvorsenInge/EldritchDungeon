---
domain: data-presets
related: [items-inventory, character-creation, combat-weapons]
---

# Data & Presets — Auto-Populated Details

## presets.json (samples)

- OWNS: array of Preset objects (name, stats, hpBonus, equipment)
- EXAMPLES: Veteran, Novice, Arcane-Adept (see src/Eldritch.Core/Data/presets.json)
- INVARIANT: PresetLoader uses case-insensitive JSON deserialize; missing file → empty array
- FLOW: CLI reads file at runtime → PresetLoader.LoadFromJson → choices presented or applied

## weapons.json

- OWNS: canonical weapon definitions used by WeaponRepository (Id, Name, Damage, CritRange, Speed, Range, Special)
- EXAMPLE entries include Dagger, Longsword, Battleaxe, Flintlock Pistol
- INVARIANT: WeaponRepository indexes by Id and Get(id) returns null if missing
- TENSION: no schema validation on load; malformed fields may silently default

## preset-loader & weapon-repo

- OWNS: lightweight deserialization helpers that assume well-formed JSON and case-insensitive properties
- READS_FROM: file path provided by CLI or caller; WeaponRepository.LoadFromJson uses File.OpenRead
- WRITES_TO: in-memory indices (WeaponRepository._byId) or returns arrays for presets
- RECOMMENDATION: add simple schema validation (required fields, id uniqueness) and fail-fast logging on load

## data-maintenance

- DECIDED: JSON chosen for human-readability and tooling interoperability
- TENSION: JSON allows easy editing but benefits from CI checks to prevent runtime surprises; consider adding unit tests that validate sample files and round-trip deserialize
## presets.json

- OWNS: named character presets with stat modifiers, HP bonus, and starting equipment
- READS_FROM: loaded by PresetLoader.LoadFromJson
- INVARIANT: entries include name, stats object, hpBonus, equipment array
- FLOW: CLI loads file → PresetLoader deserializes → CLI presents choices or applies preset

## weapons.json

- OWNS: canonical weapon definitions (Id, Name, Damage, CritRange, Speed, Range, Special)
- READS_FROM: WeaponRepository.LoadFromJson to populate in-memory lookup
- INVARIANT: weapon ids are unique; repository uses Id as key
- FLOW: LoadFromJson(path) → deserialize → index by id → used by combat and starting equipment resolution

## data-format

- OWNS: JSON format assumptions (camel or Pascal names tolerated via case-insensitive deserialize)
- TENSION: data-driven items allow rapid iteration but require validation and schema versioning
- DECIDED: use JSON for readability and tooling interoperability

## preset-loader

- OWNS: lightweight deserialization; uses System.Text.Json with case-insensitive option
- INVARIANT: failure to parse returns empty array or falls back gracefully

## weapon-repository

- OWNS: in-memory weapon lookup and lifecycle (load once at startup)
- READS_FROM: weapons.json file path (explicit load)
- WRITES_TO: internal index; exposes All and Get(id)
- TENSION: repository currently lacks live-reload or validation; consider adding schema checks
