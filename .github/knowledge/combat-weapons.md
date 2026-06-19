---
domain: combat-weapons
related: [items-inventory, character-creation]
---

# Combat & Weapons — Mental Model

## weapon-database

- OWNS: canonical weapon stats (damage, crit range, speed, range, special, durability)
- EXAMPLE: Dagger: 4 dmg, crit 18-20×2, fast, backstab +50%, durability 30
- INVARIANT: weapon entries are immutable at runtime; modifiers applied via effects

## weapon-mechanics

- OWNS: how damage is calculated (base + modifiers - armor + status effects)
- INVARIANT: damage calculation order is deterministic and idempotent
- FLOW: attack roll → hit check → critical check → damage roll → apply modifiers → apply to HP → reduce weapon durability
- TENSION: weapon durability adds resource management vs player frustration

## critical-hits

- OWNS: crit ranges and multipliers per weapon
- DECIDED: crit resolution applies before special effects (e.g., backstab) to preserve expected burst behavior
- FLOW: on crit → apply crit multiplier → then apply on-crit effects

## ranged-weapons

- OWNS: ammo, reload, range falloff, and hit penalties; supports aiming modifiers
- INVARIANT: ranged attacks consume ammo before resolving hit; misfires possible with low-quality weapons
- FLOW: check ammo → resolve hit with distance modifiers → apply damage → consume ammo

## melee-properties

- OWNS: reach, parry, armor penetration, two-handed vs off-hand rules
- INVARIANT: parry/block resolved before damage application if both sides attempt defensive actions
- TENSION: complex interactions (parry vs speed) require clear UI feedback

## weapon-mods-and-enchantments

- OWNS: attachment points and temporary buffs (fire, void, holy)
- READS FROM: item-effects and crafting recipes
- WRITES TO: modified weapon stats and visual indicators
