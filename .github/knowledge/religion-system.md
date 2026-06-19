---
domain: religion-system
related: [sanity-system, items-inventory]
---

# Religion System — Mental Model

## gods

- OWNS: god definitions (Cthulhu, Nyarlathotep, Azathoth, Yog-Sothoth, Hastur, Dagon)
- READS FROM: god data files (powers, triggers)

## favor-mechanics

- OWNS: Favor (0-100) and Anger (0-100) mechanics
- FLOW: actions → compute favor/anger delta → apply passive bonuses or penalties
- DECIDED: daily prayer grants small favor; sacrifices grant moderate favor

## god-powers

- OWNS: tiered powers unlocked at favor thresholds (25,50,75,100)
- INVARIANT: high-tier powers have significant cost or risk (sanity/anger)

## altar-systems

- OWNS: in-world altar objects and interaction rules
- READS FROM: player inventory for sacrifices
- WRITES TO: favor/anger, world state (altar consecration)

## prayers-rituals

- OWNS: ritual definitions and their game effects; may require items/time
- TENSION: rituals must balance meaningful cost vs reward to avoid trivialization
