---
domain: map-generation
related: [game-loop, map-generation]
---

# Map Generation — Mental Model

## dungeon-generation

- OWNS: floor layout algorithms (RogueSharp-based) and parameters
- FLOW: seed → generate rooms → connect corridors → validate connectivity

## room-placement

- OWNS: placement heuristics (size, sparseness, special rooms)
- INVARIANT: rooms must not overlap and must be reachable

## corridor-connection

- OWNS: corridor carving and door placement rules
- TENSION: straight corridors vs winding affects gameplay pacing

## connectivity-checks

- OWNS: flood-fill/connectivity validation and repair
- INVARIANT: every room must have path to start and exit if required

## map-seed

- OWNS: PRNG seed propagation for reproducible floors
- DECIDED: seed included in save to allow reproducible debugging

## pathfinding-integration

- OWNS: integration points with RogueSharp pathfinding for AI
- INVARIANT: pathfinder uses same map representation as renderer
