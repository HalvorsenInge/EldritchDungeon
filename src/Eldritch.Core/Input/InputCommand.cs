namespace Eldritch.Core.Input
{
    // Simple command representation for input processing and tests
    public record InputCommand(string Name);

    public static class Commands
    {
        public static readonly InputCommand MoveUp = new("MoveUp");
        public static readonly InputCommand MoveDown = new("MoveDown");
        public static readonly InputCommand MoveLeft = new("MoveLeft");
        public static readonly InputCommand MoveRight = new("MoveRight");
        public static readonly InputCommand Wait = new("Wait");
        public static readonly InputCommand Open = new("Open");
        public static readonly InputCommand Unknown = new("Unknown");
    }
}