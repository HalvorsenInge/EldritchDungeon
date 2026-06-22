---
domain: components
related: [items-inventory, combat-weapons, entity-model]
---

# Components — Auto-Populated Details

## component-base

- OWNS: marker base class used by specific components (Component)
- INVARIANT: components are stored keyed by CLR type in Entity._components

## position-component

- OWNS: X/Y tile coordinates for entity placement
- INVARIANT: coordinates are ints; renderer and engines assume integer grid
- READS_FROM: UpdateEngine, AsciiRenderer, ContentSpawner
- WRITES_TO: movement resolution updates X/Y

## intent-component

- OWNS: pending action input (InputCommand) for an entity
- FLOW: added before ResolveTurn → consumed by UpdateEngine → removed after processing
- INVARIANT: pure descriptor; contains no direct mutation logic

## loot-component

- OWNS: name of in-world loot; used as a simple source to create Item instances on pickup
- FLOW: player moves onto tile → UpdateEngine finds loot entities at position → InventoryComponent.Inventory.Add(new Item(name,"misc")) → manager.RemoveEntity(loot.Id)

## weapon-component

- OWNS: weapon stats (Damage, Durability) attached to an entity
- INVARIANT: WeaponComponent.Use reduces Durability by 1 when >0
- READS_FROM: CombatSystem prefers WeaponComponent.Damage over Str fallback

## stats-component

- OWNS: Str, Dex, Con, Int, Wis, Cha values for character calculations
- INVARIANT: default constructor sets medium defaults used by tests and deterministic flows

## health-component

- OWNS: HP and MaxHP for damage/regen; created with MaxHP initial value
- INVARIANT: HP initialized to MaxHP and expected to stay ≥0

## armor-component

- OWNS: numeric Armor value used by CombatSystem mitigation (mitig = floor(Armor/2))

## inventory-component

- OWNS: holds a reference to InventoryManager for per-entity items
- INVARIANT: InventoryManager enforces capacity (default 20) and returns bool on Add/Remove

## component-usage-pattern

- OWNS: Entity exposes AddComponent<T>, GetComponent<T>, HasComponent<T>, RemoveComponent<T>; components keyed by type
- TENSION: type-keyed component storage is simple and performant but lacks multi-instance per-type semantics
## weapon-component

- OWNS: per-entity weapon stats (Damage, Durability)
- READS FROM: used by CombatSystem when resolving attacks
- WRITES TO: Durability mutations on Use()
- INVARIANT: Durability decreases by 1 per Use when >0
- FLOW: attacker.GetComponent<WeaponComponent>() → prefer Damage over Str-based fallback

## stats-component

- OWNS: core attributes (Str, Dex, Con, Int, Wis, Cha)
- READS FROM: CombatSystem, Character creation, UI
- INVARIANT: missing stats treated as defaults where code expects ints
- TENSION: tests and deterministic systems rely heavily on defaults

## inventory-component

- OWNS: reference to InventoryManager per-entity
- READS FROM: InventoryManager for add/remove/find operations
- WRITES TO: underlying inventory collection via InventoryManager methods
- INVARIANT: InventoryManager enforces capacity and returns success bool

## position-component

- OWNS: X/Y tile coordinates for entity placement
- READS FROM: UpdateEngine, Render pipeline, ContentSpawner
- WRITES TO: updated by movement resolution
- INVARIANT: positions are integers and must be within map bounds when map is present

## health-component

- OWNS: HP current value and basic health semantics
- READS FROM: CombatSystem and damage sources
- WRITES TO: HP when damage/healing applied
- INVARIANT: HP clamped at ≥0; death handled by higher-level systems

## armor-component

- OWNS: Armor numeric value used for mitigation
- READS_FROM: CombatSystem for mitigation calculation
- INVARIANT: mitigation = floor(Armor/2)

## intent-component

- OWNS: queued player or AI action for next turn
- READS_FROM: UpdateEngine to resolve commands
- WRITES_TO: removed after processing to avoid re-execution
- INVARIANT: intent commands are pure descriptors (no direct mutation)

## loot-component

- OWNS: ephemeral loot metadata (Name, flavor) attached to in-world entity
- READS_FROM: UpdateEngine when player moves onto tile
- WRITES_TO: converted into Item and added to Inventory via InventoryComponent
