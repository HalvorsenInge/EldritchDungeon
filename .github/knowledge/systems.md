---
domain: systems
related: [game-loop, map-generation, rendering, persistence-save]
---

# Systems — Auto-Populated Details

## map

- OWNS: tile grid, neighbor iteration, width/height accessors (Map class)
- INVARIANT: tiles default to Wall on construction; Get/Set index into [_tiles,width,height]
- INVARIANT: Neighbors(x,y) yields up to 4 cardinal neighbors (N/S/E/W) constrained to map bounds
- READS_FROM: callers that query tiles for movement, rendering, connectivity
- FLOW: construct Map(width,height) → tiles initialized to Wall → generators carve Floor

## update-engine

- OWNS: deterministic turn resolution pipeline (ResolveTurn)
- READS_FROM: IntentComponent on entities, PositionComponent, Map for bounds
- WRITES_TO: PositionComponent changes, Inventory pickups via InventoryComponent, MessageLog.Add
- INVARIANT: intents processed in stable order (entity id order)
- FLOW: collect intents → order by entity id → for each intent: compute target → bounds/floor check → apply move → pick up loot → remove intent
- DECIDED: movement blocked by non-floor tiles and out-of-bounds checks

## content-spawner

- OWNS: populating map with enemy and loot entities (Populate)
- READS_FROM: Map floor tiles, RNG seed, configured enemyCount/lootCount
- WRITES_TO: creates entities with PositionComponent + Health/Stats or LootComponent
- INVARIANT: does nothing if no floor tiles; uses simple fisher-yates shuffle of floor positions
- FLOW: collect floor positions → shuffle → iterate positions → skip occupied → spawn enemies first then loot
- TENSION: single-pass index shared between enemy and loot loops; spawn order affects density/distribution

## combat-system

- OWNS: attack resolution rules and damage application
- READS_FROM: StatsComponent, WeaponComponent, ArmorComponent, HealthComponent
- WRITES_TO: HealthComponent.HP, WeaponComponent.Durability
- INVARIANT: deterministic outcome given same stats/weapon; damage ≥ 0
- FLOW: check hit (Dex compare) → compute base damage (weapon or Str/2) → apply crit → mitigate by armor → update HP → reduce weapon durability

## message-log

- OWNS: thread-safe recent messages queue (bounded to 50)
- READS_FROM: subsystems that call MessageLog.Add
- WRITES_TO: UI consumers via GetRecent and Clear
- INVARIANT: messages capped at 50 and are reversed for recent-first reads

## entity-manager

- OWNS: entity lifecycle and registry (CreateEntity, RemoveEntity, QueryByComponent)
- INVARIANT: ids increment from 1 and are unique per process instance
- FLOW: CreateEntity → allocate id → store entity → QueryByComponent filters by component presence
