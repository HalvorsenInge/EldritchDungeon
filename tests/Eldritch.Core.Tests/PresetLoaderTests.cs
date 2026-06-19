using Xunit;
using Eldritch.Core.Character;

namespace Eldritch.Core.Tests
{
    public class PresetLoaderTests
    {
        [Fact]
        public void LoadFromJson_ParsesPresets()
        {
            var json = "[ { \"name\": \"Test\", \"stats\": { \"Str\": 1 }, \"hpBonus\": 2, \"equipment\": [ \"X\" ] } ]";
            var presets = PresetLoader.LoadFromJson(json);
            Assert.Single(presets);
            Assert.Equal("Test", presets[0].Name);
            Assert.Equal(1, presets[0].Stats.Str);
            Assert.Equal(2, presets[0].HpBonus);
        }
    }
}
