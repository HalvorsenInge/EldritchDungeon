using Xunit;
using Eldritch.Core;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;
using Eldritch.Core.Systems;

namespace Eldritch.Core.Tests
{
    public class CombatRefinementsTests
    {
        [Fact]
        public void Attack_Misses_When_Lower_Dex()
        {
            var mgr = new EntityManager();
            var attacker = mgr.CreateEntity();
            attacker.AddComponent(new StatsComponent(str: 10, dex: 5));

            var defender = mgr.CreateEntity();
            defender.AddComponent(new HealthComponent(10));
            defender.AddComponent(new StatsComponent(dex: 10));

            CombatSystem.Attack(attacker, defender);

            var hp = defender.GetComponent<HealthComponent>().HP;
            Assert.Equal(10, hp);
        }

        [Fact]
        public void Attack_Critical_DoublesDamage()
        {
            var mgr = new EntityManager();
            var attacker = mgr.CreateEntity();
            attacker.AddComponent(new StatsComponent(str: 20, dex: 10));

            var defender = mgr.CreateEntity();
            defender.AddComponent(new HealthComponent(50));

            CombatSystem.Attack(attacker, defender);

            var hp = defender.GetComponent<HealthComponent>().HP;
            // Str 20 -> base dmg 10 -> critical -> 20 damage
            Assert.Equal(30, hp);
        }

        [Fact]
        public void Attack_Armor_ReducesDamage()
        {
            var mgr = new EntityManager();
            var attacker = mgr.CreateEntity();
            attacker.AddComponent(new StatsComponent(str: 10, dex: 10));

            var defender = mgr.CreateEntity();
            defender.AddComponent(new HealthComponent(20));
            defender.AddComponent(new ArmorComponent(6)); // mitig = 3

            CombatSystem.Attack(attacker, defender);

            var hp = defender.GetComponent<HealthComponent>().HP;
            // Str 10 -> base dmg 5 -> mitig 3 -> final 2 -> HP 18
            Assert.Equal(18, hp);
        }
    }
}
