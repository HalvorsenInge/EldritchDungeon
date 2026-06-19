---
domain: map-generation
related: [game-loop, map-generation]
---

# Map Generation — Mental Model

## dungeon-generation

- OWNS: floor layout algorithms (RogueSharp-based) and parameters
- FLOW: seed → generate rooms → connect corridors → validate connectivity → place content
- DECIDED: include seed in save for reproducible floors and debugging

## room-placement

- OWNS: placement heuristics (size, sparseness, special rooms: shop, altar, boss)
- INVARIANT: rooms must not overlap and must be reachable
- TENSION: randomness vs designed pacing of difficulty and loot

## corridor-connection

- OWNS: corridor carving, door placement, and choke-point rules
- FLOW: connect graph → place doors → mark special tiles (traps, secret doors)

## connectivity-checks

- OWNS: flood-fill/connectivity validation and automated repair patches
- INVARIANT: every room must have path to start and exit if required
- DECIDED: run repair step if >1% of map unreachable to ensure playability

## map-seed

- OWNS: PRNG seed propagation for reproducible floors; seed variant per floor
- INVARIANT: seed must be persisted in save metadata

## pathfinding-integration

- OWNS: integration points with RogueSharp pathfinding for AI navigation, cost maps
- INVARIANT: pathfinder uses same map representation as renderer and logic tiles

## content-placement

- OWNS: loot, enemies, traps, and special encounters placement rules
- TENSION: density of enemies vs resource availability; scale by floor depth
