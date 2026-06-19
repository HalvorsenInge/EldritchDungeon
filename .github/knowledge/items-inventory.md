---
domain: items-inventory
related: [combat-weapons, character-creation]
---

# Items & Inventory — Mental Model

## item-schema

- OWNS: canonical fields for items: id, name, type, rarity, effects, charges, stackable, equip_slot, durability
- INVARIANT: every item has a persistent unique id for save/load
- DECIDED: items include durability where applicable to support repair mechanics

## inventory-management

- OWNS: capacity, stacking rules, equip slots, quick-access slots
- DECIDED: provide 1 quick-use hotkey bar + equip slots for weapons/armor
- INVARIANT: stackable flag controls stacking and max_stack
- FLOW: pickup → try stack → if full → add to free slot → if no slot drop or refuse

## item-effects

- OWNS: effect resolution (immediate vs on-use vs passive when equipped)
- FLOW: use item → validate prerequisites → apply effects → decrement charges/durability → emit events
- TENSION: complexity of effect system vs testability; prefer small composable effects

## item-identification

- OWNS: unidentified item mechanics and identify actions; identification reveals full effect payload
- INVARIANT: identified flag persists to save
- FLOW: identify → set identified=true → update UI

## item-generation

- OWNS: loot tables, drop rates, procedural modifiers, rarity tiers
- TENSION: loot spam vs rewarding finds; use curated drops for special items

## trading-and-shops

- OWNS: merchant inventories, buy/sell rules, haggle modifiers
- READS FROM: global economy config (prices, inflation)
- WRITES TO: player gold, merchant stock

## crafting-and-repair

- OWNS: recipes, required tools, repair rules (materials, cost)
- INVARIANT: repairs cannot increase item quality beyond original
