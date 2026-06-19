namespace Eldritch.Core.Components
{
    public sealed class StatsComponent : Component
    {
        public int Str { get; set; }
        public int Dex { get; set; }
        public int Con { get; set; }
        public int Int { get; set; }
        public int Wis { get; set; }
        public int Cha { get; set; }

        public StatsComponent(int str=10, int dex=10, int con=10, int intel=10, int wis=10, int cha=10)
        {
            Str = str; Dex = dex; Con = con; Int = intel; Wis = wis; Cha = cha;
        }
    }
}