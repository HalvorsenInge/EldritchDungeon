using Xunit;
using Eldritch.Core.Character;
using Eldritch.Core.Inventory;

namespace Eldritch.Core.Tests
{
    public class CharacterPresetsTests
    {
        [Fact]
        public void ApplyPreset_Veteran_AddsEquipmentAndStats()
        {
            var rng = new System.Random(1);
            var profile = CharacterCreator.CreateCharacter(rng, Race.Human, CharacterClass.Warrior);
            profile = Presets.ApplyPreset(profile, "Veteran");

            Assert.Contains("Longsword", profile.StartingEquipment);
            Assert.True(profile.Stats.Str >= 12); // base roll plus preset
        }

        [Fact]
        public void ApplyToInventory_AddsItems()
        {
            var rng = new System.Random(2);
            var profile = CharacterCreator.CreateCharacter(rng, Race.Human, CharacterClass.Rogue);
            profile = Presets.ApplyPreset(profile, "Novice");
            var inv = new InventoryManager();
            Presets.ApplyToInventory(profile, inv);

            Assert.True(inv.Count >= 1);
            Assert.NotNull(inv.FindByName("Dagger"));
        }
    }
}
