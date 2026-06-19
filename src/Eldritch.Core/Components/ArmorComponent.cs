namespace Eldritch.Core.Components
{
    public sealed class ArmorComponent : Component
    {
        public int Armor { get; set; }
        public ArmorComponent(int armor = 0) { Armor = armor; }
    }
}