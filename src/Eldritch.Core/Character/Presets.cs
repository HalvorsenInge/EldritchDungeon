using Eldritch.Core.Components;
using Eldritch.Core.Inventory;

namespace Eldritch.Core.Character
{
    public static class Presets
    {
        public static CharacterProfile ApplyPreset(CharacterProfile profile, string preset)
        {
            // preset strings: "Veteran", "Novice", "Arcane-Adept"
            switch(preset)
            {
                case "Veteran":
                    profile.Stats.Str += 2;
                    profile.StartingEquipment = new string[] { "Longsword", "Shield", "Leather Armor" };
                    profile.MaxHP += 5; profile.HP = profile.MaxHP;
                    break;
                case "Novice":
                    profile.Stats.Con += 1;
                    profile.StartingEquipment = new string[] { "Dagger", "Cloak" };
                    break;
                case "Arcane-Adept":
                    profile.Stats.Int += 3;
                    profile.StartingEquipment = new string[] { "Wand", "Spellbook" };
                    break;
                default:
                    break;
            }
            return profile;
        }
n        public static void ApplyToInventory(CharacterProfile profile, InventoryManager inv)
        {
            if (profile.StartingEquipment == null) return;
            foreach(var name in profile.StartingEquipment)
            {
                inv.Add(new Item(name));
            }
        }    }
}
