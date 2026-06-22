using Xunit;
using System;
using Eldritch.Core.Character;

namespace Eldritch.Core.Tests
{
    public class CharacterCreationTests
    {
        [Fact]
        public void RollStats_IsDeterministicWithSeed()
        {
            var rng1 = new Random(12345);
            var rng2 = new Random(12345);

            var s1 = CharacterCreator.RollStats(rng1);
            var s2 = CharacterCreator.RollStats(rng2);

            Assert.Equal(s1.Str, s2.Str);
            Assert.Equal(s1.Dex, s2.Dex);
            Assert.Equal(s1.Con, s2.Con);
            Assert.Equal(s1.Int, s2.Int);
            Assert.Equal(s1.Wis, s2.Wis);
            Assert.Equal(s1.Cha, s2.Cha);
        }

        [Fact]
        public void RollStats_ValuesInRange()
        {
            var rng = new Random(1);
            var s = CharacterCreator.RollStats(rng);
            Assert.InRange(s.Str, 3, 18);
            Assert.InRange(s.Dex, 3, 18);
            Assert.InRange(s.Con, 3, 18);
            Assert.InRange(s.Int, 3, 18);
            Assert.InRange(s.Wis, 3, 18);
            Assert.InRange(s.Cha, 3, 18);
        }
    }
}
