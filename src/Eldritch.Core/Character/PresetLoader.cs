using System.Text.Json;
using System.Collections.Generic;

namespace Eldritch.Core.Character
{
    public class Preset
    {
        public string Name { get; set; } = "";
        public StatMods Stats { get; set; } = new StatMods();
        public int HpBonus { get; set; }
        public string[] Equipment { get; set; } = new string[0];
    }

    public class StatMods
    {
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Con { get; set; }
        public int Int { get; set; }
        public int Wis { get; set; }
        public int Cha { get; set; }
    }

    public static class PresetLoader
    {
        public static Preset[] LoadFromJson(string json)
        {
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Preset[]>(json, opts) ?? new Preset[0];
        }
    }
}
