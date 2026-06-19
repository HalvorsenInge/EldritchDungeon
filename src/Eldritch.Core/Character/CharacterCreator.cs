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
n            return new StatsComponent(
                str: Roll4d6DropLowest(),
                dex: Roll4d6DropLowest(),
                con: Roll4d6DropLowest(),
                intel: Roll4d6DropLowest(),
                wis: Roll4d6DropLowest(),
                cha: Roll4d6DropLowest());
        }
    }
}
