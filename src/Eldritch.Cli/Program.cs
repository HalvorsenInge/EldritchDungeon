using System;
using System.IO;
using System.Linq;
using Eldritch.Core.Character;
using Eldritch.Core.Inventory;

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
    }
}

