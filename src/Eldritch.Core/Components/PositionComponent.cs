namespace Eldritch.Core.Components
{
    public sealed class PositionComponent : Component
    {
        public int X { get; set; }
        public int Y { get; set; }

        public PositionComponent(int x = 0, int y = 0) { X = x; Y = y; }
    }
}