---
domain: character-creation
related: [items-inventory, persistence-save]
---

# Character Creation — Mental Model

## stat-rolling

- OWNS: initial attribute generation (STR, DEX, CON, INT, WIS, CHA)
- DECIDED: use 4d6 drop lowest per stat, reroll character if total < 75 or any stat < 6
- FLOW: roll 6× → assign to stats → apply racial modifiers → validate totals → persist choices
- TENSION: deterministic rolling for seed-based rerolls vs player expectation for randomness

## racial-modifiers

- OWNS: per-race attribute and resource modifiers; passive abilities where applicable
- EXAMPLE: Elf: DEX+2, STR-1, HP×0.9; Dwarf: STR+2, CON+2, Mana×0.8
- INVARIANT: modifiers applied before deriving HP/Mana/Sanity maxima
- FLOW: apply base roll → apply racial → apply class adjustments → compute derived stats

## class-selection

- OWNS: starting skills, proficiencies, allowed equipment, and progression archetype
- READS FROM: class templates (data files)
- WRITES TO: starting abilities, initial skill cooldowns, allowed weapon types
- TENSION: class power balance vs player freedom to multi-role

## starting-equipment

- OWNS: initial inventory by class (weapon, armor, consumables, gold)
- DECIDED: starting packages tied to class (e.g., Gunslinger → flintlock + bullets)
- INVARIANT: starting equipment must be reproducible from class template + seed

## character-save

- OWNS: converting character to persistence format for saves (includes name, class, stats, inventory)
- INVARIANT: saved character must include seed/PRNG state where relevant for reproducibility
- FLOW: on confirm → validate → serialize → write to save slot

## naming-and-backstory

- OWNS: player-chosen name and optional short background trait
- TENSION: rich backstory increases immersion but not required for mechanics

## presets-and-import

- OWNS: preset character templates for quick start
- READS FROM: presets file under data/presets.json
- DECIDED: provide 'quickstart' preset for testing and demos
