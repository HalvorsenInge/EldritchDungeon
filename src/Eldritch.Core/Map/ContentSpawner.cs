using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;
using Eldritch.Core.Inventory;

namespace Eldritch.Core.Map
{
    public static class ContentSpawner
    {
        private class EnemyDef { public string id {get;set;} = ""; public string name {get;set;} = ""; public int spawn_weight {get;set;} = 1; public int power {get;set;} = 1; public int sanity_impact {get;set;} = 0; public string[] tags {get;set;} = new string[0]; }
        private class LootDef { public string id {get;set;} = ""; public string name {get;set;} = ""; public int spawn_weight {get;set;} = 1; public int value {get;set;} = 0; public int sanity_impact {get;set;} = 0; public string[] tags {get;set;} = new string[0]; }

        // Populate map with enemies and loot, optionally using data files under src/Eldritch.Core/Data
        public static void Populate(Map map, EntityManager mgr, Random rng, int enemyCount = 5, int lootCount = 5)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            if (mgr == null) throw new ArgumentNullException(nameof(mgr));
            if (rng == null) rng = new Random();

            var floorPositions = new List<(int x,int y)>();
            for (int x = 0; x < map.Width; x++) for (int y = 0; y < map.Height; y++) if (map.Get(x,y) == TileType.Floor) floorPositions.Add((x,y));
            if (floorPositions.Count == 0) return;

            // try to load data-driven tables
            EnemyDef[] enemies = null;
            LootDef[] loots = null;
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var enemyPath = Path.Combine(baseDir, "..\\..\\..\\src\\Eldritch.Core\\Data\\enemies.json");
                var lootPath = Path.Combine(baseDir, "..\\..\\..\\src\\Eldritch.Core\\Data\\loot.json");
                if (File.Exists(enemyPath))
                {
                    using var s = File.OpenRead(enemyPath);
                    enemies = JsonSerializer.Deserialize<EnemyDef[]>(s, new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
                }
                if (File.Exists(lootPath))
                {
                    using var s = File.OpenRead(lootPath);
                    loots = JsonSerializer.Deserialize<LootDef[]>(s, new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
                }
            }
            catch { /* ignore and fallback to default behavior */ }

            // shuffle positions
            for (int i = floorPositions.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = floorPositions[i]; floorPositions[i] = floorPositions[j]; floorPositions[j] = tmp;
            }

            int idx = 0;
            bool IsOccupied(int x,int y) => mgr.QueryByComponent<PositionComponent>().Any(e => { var p = e.GetComponent<PositionComponent>(); return p != null && p.X == x && p.Y == y; });

            // spawn enemies
            int spawnedEnemies = 0;
            if (enemies != null && enemies.Length > 0)
            {
                // build weighted list
                var weighted = new List<EnemyDef>();
                foreach(var ed in enemies) for(int w=0; w<Math.Max(1, ed.spawn_weight); w++) weighted.Add(ed);

                int attempts = 0;
                while (spawnedEnemies < enemyCount && attempts < floorPositions.Count*3)
                {
                    attempts++;
                    if (idx >= floorPositions.Count) idx = 0; // wrap around to try other positions
                    var (x,y) = floorPositions[idx++];
                    if (IsOccupied(x,y)) continue;
                    var sel = weighted[rng.Next(weighted.Count)];
                    var e = mgr.CreateEntity();
                    e.AddComponent(new PositionComponent(x,y));
                    e.AddComponent(new HealthComponent(Math.Max(1, sel.power * 2)));
                    e.AddComponent(new StatsComponent(str: Math.Max(1, sel.power*2), dex: Math.Max(1, sel.power)));
                    spawnedEnemies++;
                }
            }
            else
            {
                while (spawnedEnemies < enemyCount && idx < floorPositions.Count)
                {
                    var (x,y) = floorPositions[idx++];
                    if (IsOccupied(x,y)) continue;
                    var e = mgr.CreateEntity();
                    e.AddComponent(new PositionComponent(x,y));
                    e.AddComponent(new HealthComponent(10));
                    e.AddComponent(new StatsComponent(str: 8, dex: 6, con: 8));
                    spawnedEnemies++;
                }
            }

            // spawn loot
            int spawnedLoot = 0;
            if (loots != null && loots.Length > 0)
            {
                var weighted = new List<LootDef>();
                foreach(var ld in loots) for(int w=0; w<Math.Max(1, ld.spawn_weight); w++) weighted.Add(ld);
                int attempts = 0;
                while (spawnedLoot < lootCount && attempts < floorPositions.Count*3)
                {
                    attempts++;
                    if (idx >= floorPositions.Count) idx = 0;
                    var (x,y) = floorPositions[idx++];
                    if (IsOccupied(x,y)) continue;
                    var sel = weighted[rng.Next(weighted.Count)];
                    var e = mgr.CreateEntity();
                    e.AddComponent(new PositionComponent(x,y));
                    e.AddComponent(new LootComponent(sel.name));
                    spawnedLoot++;
                }
            }
            else
            {
                var lootNames = new string[] { "Gold", "Potion", "Scroll", "Gem", "Dagger" };
                while (spawnedLoot < lootCount && idx < floorPositions.Count)
                {
                    var (x,y) = floorPositions[idx++];
                    if (IsOccupied(x,y)) continue;
                    var e = mgr.CreateEntity();
                    e.AddComponent(new PositionComponent(x,y));
                    var name = lootNames[rng.Next(lootNames.Length)];
                    e.AddComponent(new LootComponent(name));
                    spawnedLoot++;
                }
            }
        }
    }
}
