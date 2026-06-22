using Xunit;
using System;
using Eldritch.Core.Character;
using Eldritch.Core.Inventory;

namespace Eldritch.Core.Tests
{
    public class CharacterWizardTests
    {
        [Fact]
        public void CreateWithPreset_AppliesJsonPreset()
        {
            var rng = new Random(5);
            var json = "[ { \"name\": \"TinyTest\", \"stats\": { \"Str\": 2 }, \"hpBonus\": 3, \"equipment\": [ \"Stick\" ] } ]";
            var presets = PresetLoader.LoadFromJson(json);
            var preset = presets[0];

            var profile = CharacterCreationWizard.CreateWithPreset(rng, Race.Human, CharacterClass.Mage, preset);
            Assert.True(profile.Stats.Str >= 2);
            Assert.Contains("Stick", profile.StartingEquipment);
            Assert.Equal(profile.MaxHP, profile.HP);
            var inv = new InventoryManager();
            CharacterCreationWizard.ApplyPresetToInventory(profile, inv);
            Assert.NotNull(inv.FindByName("Stick"));
        }
    }
}
