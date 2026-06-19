using System;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;

namespace Eldritch.Core.Systems
{
    public static class CombatSystem
    {
        // Simple deterministic attack: damage = max(1, Str / 2)
        public static void Attack(Entity attacker, Entity defender)
        {
            if (attacker == null || defender == null) return;
            var atkStats = attacker.GetComponent<StatsComponent>();
            var defHealth = defender.GetComponent<HealthComponent>();
            if (defHealth == null) return;

            int str = atkStats?.Str ?? 1;
            int damage = Math.Max(1, str / 2);
            defHealth.HP = Math.Max(0, defHealth.HP - damage);
        }
    }
}