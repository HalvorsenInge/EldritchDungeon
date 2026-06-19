---
domain: religion-system
related: [sanity-system, items-inventory]
---

# Religion System — Mental Model

## gods

- OWNS: god definitions (Cthulhu, Nyarlathotep, Azathoth, Yog-Sothoth, Hastur, Dagon)
- READS FROM: god data files (powers, triggers, flavor text)
- OWNS: god alignments and enemy/ally relationships for factions
- TENSION: gods influencing world state vs player agency—limit direct world rewrites

## favor-mechanics

- OWNS: Favor (0-100) and Anger (0-100) mechanics and decay rates
- FLOW: actions → compute favor/anger delta → apply passive bonuses or penalties → persist changes
- DECIDED: daily prayer grants small favor; sacrifices grant moderate favor; betrayal increases anger sharply
- INVARIANT: Favor and Anger are independent but both influence god responses

## god-powers

- OWNS: tiered powers unlocked at favor thresholds (25,50,75,100)
- INVARIANT: high-tier powers have significant cost or risk (sanity/anger, world consequences)
- FLOW: when invoked → check favor threshold → resolve power → apply consequences and possible backlash

## altar-systems

- OWNS: in-world altar objects and interaction rules (sacrifice, consecrate, desecrate)
- READS FROM: player inventory for sacrifices, world time for ritual durations
- WRITES TO: favor/anger, world state (altar consecration), NPC reactions
- TENSION: making altars persistent anchors vs ephemeral bonuses

## prayers-rituals

- OWNS: ritual definitions and their game effects; may require items/time and have cooldowns
- DECIDED: some rituals require group of followers or specific locations to balance power
- TENSION: rituals that are too cheap trivialize progression; require meaningful cost

## religion-npcs

- OWNS: NPC behavior tied to gods (priests, cultists, worshippers)
- READS FROM: god favor state, player actions
- WRITES TO: world events (summons, protests) and reputation tracking
