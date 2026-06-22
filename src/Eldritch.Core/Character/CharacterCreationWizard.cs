using System;
using Eldritch.Core.Inventory;

namespace Eldritch.Core.Character
{
    public static class CharacterCreationWizard
    {
        // Create character and optionally apply a preset (from Preset object)
        public static CharacterProfile CreateWithPreset(Random rng, Race race, CharacterClass cls, Preset? preset = null)
        {
            var profile = CharacterCreator.CreateCharacter(rng, race, cls);
            if (preset == null) return profile;

            // Apply stat mods
            profile.Stats.Str += preset.Stats.Str;
            profile.Stats.Dex += preset.Stats.Dex;
            profile.Stats.Con += preset.Stats.Con;
            profile.Stats.Int += preset.Stats.Int;
            profile.Stats.Wis += preset.Stats.Wis;
            profile.Stats.Cha += preset.Stats.Cha;

            // Equipment
            var eq = new string[(profile.StartingEquipment?.Length ?? 0) + (preset.Equipment?.Length ?? 0)];
            int i = 0;
            if (profile.StartingEquipment != null) { foreach(var e in profile.StartingEquipment) eq[i++] = e; }
            if (preset.Equipment != null) { foreach(var e in preset.Equipment) eq[i++] = e; }
            profile.StartingEquipment = eq;

            // HP bonus
            profile.MaxHP += preset.HpBonus;
            profile.HP = profile.MaxHP;

            return profile;
        }

        public static void ApplyPresetToInventory(CharacterProfile profile, InventoryManager inv)
        {
            if (profile.StartingEquipment == null) return;
            foreach(var name in profile.StartingEquipment)
            {
                inv.Add(new Item(name, "Misc"));
            }
        }
    }
}
