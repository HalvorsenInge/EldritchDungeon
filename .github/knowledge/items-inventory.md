---
domain: items-inventory
related: [combat-weapons, character-creation]
---

# Items & Inventory — Mental Model

## item-schema

- OWNS: canonical fields for items: id, name, type, rarity, effects, charges, stackable, equip_slot
- INVARIANT: every item has a persistent unique id for save/load

## inventory-management

- OWNS: capacity, stacking rules, equip slots, weight (if used)
- DECIDED: starter inventories are class-defined and obey stack limits

## item-effects

- OWNS: effect resolution (immediate vs on-use vs passive when equipped)
- FLOW: apply effect → modify target stats/state → log event

## item-identification

- OWNS: unidentified item mechanics and identify actions (Yog-Sothoth auto-identify at tier)
- INVARIANT: unidentified items must expose enough metadata for UI but not effects

## item-generation

- OWNS: loot tables, drop rates, procedural modifiers
- TENSION: deterministic seeds vs randomness for repeatable runs
