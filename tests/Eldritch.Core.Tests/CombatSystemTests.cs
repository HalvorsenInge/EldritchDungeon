using Xunit;
using Eldritch.Core;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;
using Eldritch.Core.Systems;

namespace Eldritch.Core.Tests
{
    public class CombatSystemTests
    {
        [Fact]
        public void Attack_ReducesDefenderHP()
        {
            var mgr = new EntityManager();
            var attacker = mgr.CreateEntity();
            attacker.AddComponent(new StatsComponent(str: 10)); // Str 10 -> damage = 5

            var defender = mgr.CreateEntity();
            defender.AddComponent(new HealthComponent(20));

            CombatSystem.Attack(attacker, defender);

            var hp = defender.GetComponent<HealthComponent>().HP;
            Assert.Equal(15, hp);
        }

        [Fact]
        public void Attack_DoesNotGoBelowZero()
        {
            var mgr = new EntityManager();
            var attacker = mgr.CreateEntity();
            attacker.AddComponent(new StatsComponent(str: 100)); // big damage

            var defender = mgr.CreateEntity();
            defender.AddComponent(new HealthComponent(3));

            CombatSystem.Attack(attacker, defender);

            var hp = defender.GetComponent<HealthComponent>().HP;
            Assert.Equal(0, hp);
        }
    }
}
