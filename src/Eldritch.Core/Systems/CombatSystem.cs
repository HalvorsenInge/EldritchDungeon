using System;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;

namespace Eldritch.Core.Systems
{
    public static class CombatSystem
    {
        // Deterministic rules:
        // - Hit if attacker.Dex >= defender.Dex (missing stats treated as 0)
        // - Critical if attacker.Str >= 20 -> double damage
        // - Base damage = max(1, Str / 2)
        // - Armor mitigation = floor(Armor / 2) subtracted from damage (min 0)
        public static void Attack(Entity attacker, Entity defender)
        {
            if (attacker == null || defender == null) return;
            var atkStats = attacker.GetComponent<StatsComponent>();
            var defStats = defender.GetComponent<StatsComponent>();
            var defHealth = defender.GetComponent<HealthComponent>();
            var defArmor = defender.GetComponent<ArmorComponent>();
            if (defHealth == null) return;
n            int attackerDex = atkStats?.Dex ?? 0;
            int defenderDex = defStats?.Dex ?? 0;

            // Miss if attacker is less dexterous than defender
            if (attackerDex < defenderDex) return;

            int str = atkStats?.Str ?? 1;
            int damage = Math.Max(1, str / 2);
            bool critical = str >= 20;
            if (critical) damage *= 2;

            int armor = defArmor?.Armor ?? 0;
            int mitig = armor / 2;
            damage = Math.Max(0, damage - mitig);
n            defHealth.HP = Math.Max(0, defHealth.HP - damage);
        }
    }
}