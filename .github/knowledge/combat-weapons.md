---
domain: combat-weapons
related: [items-inventory, character-creation]
---

# Combat & Weapons — Mental Model

## weapon-database

- OWNS: canonical weapon stats (damage, crit range, speed, range, special)
- EXAMPLE: Dagger: 4 dmg, crit 18-20×2, fast, backstab +50%

## weapon-mechanics

- OWNS: how damage is calculated (base + modifiers - armor)
- INVARIANT: damage calculation order is deterministic and idempotent
- FLOW: attack roll → hit check → damage roll → apply modifiers → apply to HP

## critical-hits

- OWNS: crit ranges and multipliers per weapon
- DECIDED: crit resolution applies before special effects (e.g., backstab)

## ranged-weapons

- OWNS: ammo, reload, range falloff, and hit penalties
- INVARIANT: ranged attacks consume ammo before resolving hit

## melee-properties

- OWNS: reach, parry, armor penetration, and speed interactions
- TENSION: balancing weapon speed vs damage to favor meaningful choices
