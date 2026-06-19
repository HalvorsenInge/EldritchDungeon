using Xunit;
using Eldritch.Core.Weapons;
using System.IO;

namespace Eldritch.Core.Tests
{
    public class WeaponRepositoryTests
    {
        [Fact]
        public void LoadFromJson_PopulatesWeapons()
        {
            var repo = new WeaponRepository();
            // Locate weapons.json by walking up parent directories until found
            var current = Directory.GetCurrentDirectory();
            string? path = null;
            var dir = new DirectoryInfo(current);
            while (dir != null)
            {
                var candidate = Path.Combine(dir.FullName, "src", "Eldritch.Core", "Data", "weapons.json");
                if (File.Exists(candidate)) { path = candidate; break; }
                dir = dir.Parent;
            }
            Assert.False(string.IsNullOrEmpty(path), "weapons.json not found in repo");
            repo.LoadFromJson(path);
            var dagger = repo.Get("dagger");
            Assert.NotNull(dagger);
            Assert.Equal("Dagger", dagger.Name);
            Assert.Equal(4, dagger.Damage);

            var pistol = repo.Get("flintlock");
            Assert.NotNull(pistol);
            Assert.Equal(6, pistol.Range);
        }
    }
}