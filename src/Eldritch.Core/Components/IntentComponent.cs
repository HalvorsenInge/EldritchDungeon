using Eldritch.Core.Input;

namespace Eldritch.Core.Components
{
    public sealed class IntentComponent : Component
    {
        public InputCommand Command { get; set; }
        public IntentComponent(InputCommand cmd) => Command = cmd;
    }
}