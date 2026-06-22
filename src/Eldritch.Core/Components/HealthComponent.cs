namespace Eldritch.Core.Components
{
    public sealed class HealthComponent : Component
    {
        public int HP { get; set; }
        public int MaxHP { get; set; }

        public HealthComponent(int maxHp)
        {
            MaxHP = maxHp;
            HP = maxHp;
        }
    }
}