---
domain: sanity-system
related: [items-inventory, religion-system]
---

# Sanity System — Mental Model

## sanity-model

- OWNS: CurrentSanity (0-100) and MaxSanity calculation
- DECIDED: MaxSanity = 100 + (WIS modifier × 10)
- INVARIANT: sanity is clamped to [0, MaxSanity]

## sanity-damage-sources

- OWNS: list of sources and damage ranges (monsters, tomes, environment)
- EXAMPLES: Deep One: 5-15, Mi-Go: 10-20, Shoggoth: 25-40, Great Old One: 100

## sanity-healing

- OWNS: healing items and their side-effects (addiction, stat penalties)
- EXAMPLES: Sanctified Water +30 (safe); Mindcrust +50 (addiction, -10 INT)
- TENSION: powerful heals introduce addiction/tension vs player agency

## addiction-system

- OWNS: AddictionLevel (0-100), thresholds, craving/withdrawal behavior
- INVARIANT: Addiction ≥50 considered 'addicted' and applies withdrawal penalties

## hallucination-events

- OWNS: transient fake sensory events triggered below thresholds
- INVARIANT: hallucinations must not mutate persistent world state (only perception)
- FLOW: sanity drop → check thresholds → queue hallucination events → render as messages
