---
domain: character-creation
related: [items-inventory, persistence-save]
---

# Character Creation — Mental Model

## stat-rolling

- OWNS: initial attribute generation (STR, DEX, CON, INT, WIS, CHA)
- DECIDED: use 4d6 drop lowest per stat, reroll character if total < 75 or any stat < 6
- FLOW: roll 6× → assign to stats → apply racial modifiers → validate totals

## racial-modifiers

- OWNS: per-race attribute and resource modifiers
- EXAMPLE: Elf: DEX+2, STR-1, HP×0.9; Dwarf: STR+2, CON+2, Mana×0.8
- INVARIANT: modifiers applied before deriving HP/Mana/Sanity maxima

## class-selection

- OWNS: starting skills, proficiencies, and allowed equipment
- READS FROM: class templates (default load from data files)
- WRITES TO: starting abilities, level progression seeds

## starting-equipment

- OWNS: initial inventory by class (weapon, armor, consumables, gold)
- DECIDED: starting packages tied to class (e.g., Gunslinger → flintlock + bullets)

## character-save

- OWNS: converting character to persistence format for saves
- INVARIANT: saved character must include seed/seeded PRNG state where relevant for reproducibility
