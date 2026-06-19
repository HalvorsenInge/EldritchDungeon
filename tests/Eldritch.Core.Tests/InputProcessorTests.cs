using Xunit;
using Eldritch.Core.Input;

namespace Eldritch.Core.Tests
{
    public class InputProcessorTests
    {
        [Theory]
        [InlineData("w", "MoveUp")]
        [InlineData("s", "MoveDown")]
        [InlineData("a", "MoveLeft")]
        [InlineData("d", "MoveRight")]
        [InlineData(".", "Wait")]
        [InlineData(",", "Wait")]
        [InlineData("o", "Open")]
        [InlineData("x", "Unknown")]
        public void Parse_ReturnsExpectedCommand(string key, string expected)
        {
            var p = new InputProcessor();
            var cmd = p.Parse(key);
            Assert.Equal(expected, cmd.Name);
        }
    }
}