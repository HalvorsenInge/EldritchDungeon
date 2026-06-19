using System;

namespace Eldritch.Core.Input
{
    public class InputProcessor
    {
        // Translate a raw key (single-char string) into a canonical InputCommand
        public InputCommand Parse(string key)
        {
            if (string.IsNullOrEmpty(key)) return Commands.Unknown;
            switch (key.Trim().ToLowerInvariant())
            {
                case "w": return Commands.MoveUp;
                case "s": return Commands.MoveDown;
                case "a": return Commands.MoveLeft;
                case "d": return Commands.MoveRight;
                case ".": return Commands.Wait;
                case ",": return Commands.Wait;
                case "o": return Commands.Open;
                case "enter": return Commands.Open;
                default: return Commands.Unknown;
            }
        }
    }
}