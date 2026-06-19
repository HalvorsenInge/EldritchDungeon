using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Eldritch.Core.Weapons
{
    public class WeaponRepository
    {
        private readonly Dictionary<string, Weapon> _byId = new();

        public IEnumerable<Weapon> All => _byId.Values;

        public void LoadFromJson(string path)
        {
            using var s = File.OpenRead(path);
            var arr = JsonSerializer.Deserialize<Weapon[]>(s, new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
            if(arr==null) return;
            foreach(var w in arr) _byId[w.Id]=w;
        }

        public Weapon? Get(string id) => _byId.TryGetValue(id, out var w) ? w : null;
    }
}