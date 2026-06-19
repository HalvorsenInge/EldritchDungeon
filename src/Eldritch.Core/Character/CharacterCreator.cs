using System;
using Eldritch.Core.Components;

namespace Eldritch.Core.Character
{
    public static class CharacterCreator
    {
        // Roll stats using 4d6 drop lowest for STR, DEX, CON, INT, WIS, CHA        
        public static StatsComponent RollStats(Random rng)
        {
            int Roll4d6DropLowest()
            {
                int a = rng.Next(1,7);
                int b = rng.Next(1,7);
                int c = rng.Next(1,7);
                int d = rng.Next(1,7);
                int sum = a + b + c + d - Math.Min(Math.Min(a,b), Math.Min(c,d));
                return sum;
            }
            return new StatsComponent(
                str: Roll4d6DropLowest(),
                dex: Roll4d6DropLowest(),
                con: Roll4d6DropLowest(),
                intel: Roll4d6DropLowest(),
                wis: Roll4d6DropLowest(),
                cha: Roll4d6DropLowest());
        }

        // Create a full character profile: apply race modifiers, class presets and compute HP/equipment
        public static CharacterProfile CreateCharacter(Random rng, Race race, CharacterClass cls)
        {
            var stats = RollStats(rng);

            // Apply simple race modifiers
            switch(race)
            {
                case Race.Elf:
                    stats.Dex += 2;
                    stats.Con -= 2;
                    break;
                case Race.Dwarf:
                    stats.Con += 2;
                    stats.Cha -= 2;
                    break;
                case Race.Human:
                default:
                    break;
            }

            // Starting HP: base 10 + (Con - 10)/2 (integer division)
            int maxHp = 10 + ((stats.Con - 10) / 2);
            if (maxHp < 1) maxHp = 1;

            string[] equipment = cls switch
            {
                CharacterClass.Warrior => new string[] { "Sword", "Shield" },
                CharacterClass.Rogue => new string[] { "Dagger", "Lockpick" },
                CharacterClass.Mage => new string[] { "Staff", "Spellbook" },
                _ => new string[0]
            };

            return new CharacterProfile {
                Stats = stats,
                Race = race,
                Class = cls,
                MaxHP = maxHp,
                HP = maxHp,
                StartingEquipment = equipment
            };
        }
    }
}
