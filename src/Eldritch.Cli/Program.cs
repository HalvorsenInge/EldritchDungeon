using System;
using System.IO;
using System.Linq;
using System.Threading;
using Eldritch.Core;
using Eldritch.Core.Character;
using Eldritch.Core.Inventory;
using Eldritch.Core.Map;
using Eldritch.Core.Rendering;
using Eldritch.Core.Components;
using Eldritch.Core.Entities;

namespace Eldritch.Cli
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            // Parse simple --key[=value] args
            var options = ParseArgs(args);
            if (options.ContainsKey("help") || options.ContainsKey("h"))
            {
                PrintHelp();
                return 0;
            }

            Console.WriteLine("Eldritch Dungeon - Character Creator (CLI)");

            // Load presets
            Preset[] presets = new Preset[0];
            var presetPath = Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\src\\Eldritch.Core\\Data\\presets.json");
            if (File.Exists(presetPath))
            {
                var json = File.ReadAllText(presetPath);
                presets = PresetLoader.LoadFromJson(json);
            }

            bool nonInteractive = options.ContainsKey("non-interactive") || options.ContainsKey("noninteractive") || options.ContainsKey("ni");

            // Seed
            int? seed = null;
            if (options.TryGetValue("seed", out var seedStr) && int.TryParse(seedStr, out var sval)) seed = sval;

            // Map and viewport sizes
            int mapWidth = 80, mapHeight = 40;
            int viewportWidth = 40, viewportHeight = 20;
            if (options.TryGetValue("map-width", out var mw) && int.TryParse(mw, out var mi)) mapWidth = Math.Max(10, mi);
            if (options.TryGetValue("map-height", out var mh) && int.TryParse(mh, out var mbi)) mapHeight = Math.Max(10, mbi);
            if (options.TryGetValue("viewport-width", out var vwStr) && int.TryParse(vwStr, out var vwi)) viewportWidth = Math.Max(5, vwi);
            if (options.TryGetValue("viewport-height", out var vhStr) && int.TryParse(vhStr, out var vhi)) viewportHeight = Math.Max(5, vhi);

            // Race, Class, Preset
            Race? raceOpt = null;
            if (options.TryGetValue("race", out var rstr) && Enum.TryParse<Race>(rstr, true, out var r)) raceOpt = r;

            CharacterClass? classOpt = null;
            if (options.TryGetValue("class", out var cstr) && Enum.TryParse<CharacterClass>(cstr, true, out var c)) classOpt = c;

            Preset? chosenPreset = null;
            if (options.TryGetValue("preset", out var pstr) && presets != null && presets.Length > 0)
            {
                chosenPreset = presets.FirstOrDefault(p => string.Equals(p.Name, pstr, StringComparison.OrdinalIgnoreCase));
            }

            bool applyToInventory = options.ContainsKey("apply") || options.ContainsKey("a");

            Random rng = seed.HasValue ? new Random(seed.Value) : new Random();

            bool playMode = options.ContainsKey("play") || options.ContainsKey("game");

            // Interactive fallback if needed
            if (!nonInteractive)
            {
                // choose race if not provided
                if (raceOpt == null)
                {
                    raceOpt = PromptEnum<Race>("Choose race:");
                }

                if (classOpt == null)
                {
                    classOpt = PromptEnum<CharacterClass>("Choose class:");
                }

                if (chosenPreset == null && presets.Length > 0)
                {
                    Console.WriteLine("Choose preset (or 0 for none):");
                    Console.WriteLine("  0) None");
                    for (int i = 0; i < presets.Length; i++) Console.WriteLine($"  {i+1}) {presets[i].Name}");
                    int psel = PromptNumber(0, presets.Length) - 1;
                    if (psel >= 0) chosenPreset = presets[psel];
                }

                // Offer to launch small game screen
                Console.WriteLine("Launch game screen? (Y/n)");
                var launch = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(launch) || launch.Trim().ToLowerInvariant().StartsWith("y")) playMode = true;
            }
            else
            {
                // Non-interactive: must have race and class
                if (raceOpt == null || classOpt == null)
                {
                    Console.Error.WriteLine("Non-interactive mode requires --race and --class to be specified.");
                    return 2;
                }
            }

            var profile = CharacterCreationWizard.CreateWithPreset(rng, raceOpt.Value, classOpt.Value, chosenPreset);

            PrintProfile(profile);

            if (applyToInventory)
            {
                var inv = new InventoryManager();
                CharacterCreationWizard.ApplyPresetToInventory(profile, inv);
                Console.WriteLine($"Inventory contains {inv.Count} items.");
            }

            if (playMode)
            {
                PlayLoop(profile);
            }

            return 0;
        }

        static void PrintHelp()
        {
            Console.WriteLine("Usage: dotnet run --project src\\Eldritch.Cli -- [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  --seed=N            Set RNG seed for deterministic rolls");
            Console.WriteLine("  --race=Race         Choose race (Human, Elf, Dwarf)");
            Console.WriteLine("  --class=Class       Choose class (Warrior, Rogue, Mage)");
            Console.WriteLine("  --preset=Name       Apply preset by name from presets.json");
            Console.WriteLine("  --apply             Apply starting equipment to an inventory (non-interactive)");
            Console.WriteLine("  --non-interactive   Run without prompts (requires --race and --class)");
            Console.WriteLine("  --viewport-width=N  Viewport width in characters (default 40)");
            Console.WriteLine("  --viewport-height=N Viewport height in characters (default 20)");
            Console.WriteLine("  --map-width=N       Map width (default 80)");
            Console.WriteLine("  --map-height=N      Map height (default 40)");
            Console.WriteLine("  --play              Launch the simple game screen immediately");
            Console.WriteLine("  --help              Show this help");
        }

        static void PrintProfile(CharacterProfile profile)
        {
            Console.WriteLine("\n--- Character Preview ---");
            Console.WriteLine($"Race: {profile.Race}");
            Console.WriteLine($"Class: {profile.Class}");
            Console.WriteLine($"HP: {profile.HP}/{profile.MaxHP}");
            Console.WriteLine($"Stats: STR={profile.Stats.Str} DEX={profile.Stats.Dex} CON={profile.Stats.Con} INT={profile.Stats.Int} WIS={profile.Stats.Wis} CHA={profile.Stats.Cha}");
            Console.WriteLine($"Starting equipment: {string.Join(", ", profile.StartingEquipment ?? Array.Empty<string>())}");
        }

        static int PromptNumber(int min, int max)
        {
            while (true)
            {
                Console.Write($"Enter number ({min}-{max}): ");
                var line = Console.ReadLine();
                if (int.TryParse(line, out var v) && v >= min && v <= max) return v;
                Console.WriteLine("Invalid selection, try again.");
            }
        }

        static T PromptEnum<T>(string prompt) where T : struct
        {
            var values = Enum.GetValues(typeof(T));
            Console.WriteLine(prompt);
            for (int i = 0; i < values.Length; i++) Console.WriteLine($"  {i+1}) {values.GetValue(i)}");
            var sel = PromptNumber(1, values.Length) - 1;
            return (T)values.GetValue(sel);
        }

        static System.Collections.Generic.Dictionary<string,string> ParseArgs(string[] args)
        {
            var dict = new System.Collections.Generic.Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                if (!a.StartsWith("--")) continue;
                var keyVal = a.Substring(2);
                string key, val;
                if (keyVal.Contains('='))
                {
                    var parts = keyVal.Split('=', 2);
                    key = parts[0]; val = parts[1];
                }
                else
                {
                    key = keyVal;
                    // look ahead for value
                    if (i + 1 < args.Length && !args[i+1].StartsWith("-")) { val = args[i+1]; i++; }
                    else { val = ""; }
                }
                dict[key] = val;
            }
            return dict;
        }

        static void PlayLoop(CharacterProfile profile, int mapWidth = 80, int mapHeight = 40, int viewportWidth = 40, int viewportHeight = 20)
        {
            var map = new Map(mapWidth, mapHeight);
            // carve a simple room inside borders
            for (int x = 1; x < mapWidth - 1; x++) for (int y = 1; y < mapHeight - 1; y++) map.Set(x, y, TileType.Floor);

            var manager = new EntityManager();
            var player = manager.CreateEntity();
            var startX = mapWidth / 2; var startY = mapHeight / 2;
            player.AddComponent(new PositionComponent(startX, startY));

            Console.Clear();
            int vx = Math.Max(0, startX - viewportWidth / 2);
            int vy = Math.Max(0, startY - viewportHeight / 2);

            while (true)
            {
                // clamp viewport
                vx = Math.Min(Math.Max(0, vx), Math.Max(0, mapWidth - viewportWidth));
                vy = Math.Min(Math.Max(0, vy), Math.Max(0, mapHeight - viewportHeight));

                var buf = AsciiRenderer.RenderViewport(map, manager, vx, vy, viewportWidth, viewportHeight);
                Console.SetCursorPosition(0, 0);
                for (int y = 0; y < viewportHeight; y++)
                {
                    for (int x = 0; x < viewportWidth; x++) Console.Write(buf[x, y]);
                    Console.WriteLine();
                }

                Console.WriteLine($"HP: {profile.HP}/{profile.MaxHP}  STR:{profile.Stats.Str} DEX:{profile.Stats.Dex}");
                Console.WriteLine("Use arrows or WASD to move, Q to quit.");

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Q) break;
                int dx = 0, dy = 0;
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow: case ConsoleKey.A: dx = -1; break;
                    case ConsoleKey.RightArrow: case ConsoleKey.D: dx = 1; break;
                    case ConsoleKey.UpArrow: case ConsoleKey.W: dy = -1; break;
                    case ConsoleKey.DownArrow: case ConsoleKey.S: dy = 1; break;
                }

                if (dx != 0 || dy != 0)
                {
                    var pos = player.GetComponent<PositionComponent>();
                    if (pos != null)
                    {
                        var nx = pos.X + dx; var ny = pos.Y + dy;
                        if (nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height && map.Get(nx, ny) == TileType.Floor)
                        {
                            pos.X = nx; pos.Y = ny;

                            // center viewport on player
                            vx = pos.X - viewportWidth / 2;
                            vy = pos.Y - viewportHeight / 2;
                        }
                    }
                }
            }
        }
    }
}

