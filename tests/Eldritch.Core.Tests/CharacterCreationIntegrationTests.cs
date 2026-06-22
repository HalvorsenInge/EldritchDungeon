using Xunit;
using System;
using Eldritch.Core.Character;

namespace Eldritch.Core.Tests
{
    public class CharacterCreationIntegrationTests
    {
        [Fact]
        public void CreateCharacter_AppliesRaceModifiers_And_Equipment()
        {
            var rng = new Random(42);
            var profile = CharacterCreator.CreateCharacter(rng, Race.Elf, CharacterClass.Rogue);

            // Elf gets +2 Dex, -2 Con
            Assert.InRange(profile.Stats.Dex, 3, 20);
            Assert.Contains("Dagger", profile.StartingEquipment);
            Assert.Equal(CharacterClass.Rogue, profile.Class);
            Assert.Equal(Race.Elf, profile.Race);
            Assert.True(profile.MaxHP >= 1);
        }
    }
}
