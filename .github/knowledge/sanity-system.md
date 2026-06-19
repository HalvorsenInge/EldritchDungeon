---
domain: sanity-system
related: [items-inventory, religion-system]
---

# Sanity System — Mental Model

## sanity-model

- OWNS: CurrentSanity (0-100) and MaxSanity calculation
- DECIDED: MaxSanity = 100 + (WIS modifier × 10)
- INVARIANT: sanity is clamped to [0, MaxSanity]
- OWNS: sanity resistances and modifiers (race/class/god bonuses)
- FLOW: apply source → compute resistances → clamp → trigger thresholds

## sanity-damage-sources

- OWNS: list of sources and damage ranges (monsters, tomes, environment)
- EXAMPLES: Deep One: 5-15, Mi-Go: 10-20, Shoggoth: 25-40, Great Old One: 100
- TENSION: balancing encounter design so sanity loss is meaningful but recoverable

## sanity-healing

- OWNS: healing items and their side-effects (addiction, stat penalties)
- EXAMPLES: Sanctified Water +30 (safe); Mindcrust +50 (addiction, -10 INT)
- INVARIANT: healing items apply both immediate and potential long-term penalties
- FLOW: use item → apply heal → apply side-effect (addiction/timers) → log event

## addiction-system

- OWNS: AddictionLevel (0-100), thresholds, craving/withdrawal behavior
- INVARIANT: Addiction ≥50 considered 'addicted' and applies withdrawal penalties
- FLOW: repeated use → increase tolerance → increase addiction score → apply withdrawal when abstinent

## hallucination-events

- OWNS: transient fake sensory events triggered below thresholds
- INVARIANT: hallucinations must not mutate persistent world state (only perception)
- FLOW: sanity drop → check thresholds → queue hallucination events → render as messages; track frequency to avoid spam
- TENSION: hallucinations as flavor vs gameplay-affecting events (keep mostly perceptual unless designed otherwise)

## sanity-states

- OWNS: named bands (Stable, Fractured, Unraveling, Broken) and associated mechanics
- INVARIANT: transitions only when crossing thresholds; hysteresis allowed to prevent flapping
