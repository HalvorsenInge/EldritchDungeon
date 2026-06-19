using Eldritch.Core.Components;

namespace Eldritch.Core.Character
{
    public enum Race { Human, Elf, Dwarf }
    public enum CharacterClass { Warrior, Rogue, Mage }

    public class CharacterProfile
    {
        public StatsComponent Stats { get; set; }
        public Race Race { get; set; }
        public CharacterClass Class { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public string[] StartingEquipment { get; set; }
    }
}