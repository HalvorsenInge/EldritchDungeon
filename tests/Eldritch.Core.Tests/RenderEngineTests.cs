using Xunit;
using Eldritch.Core.Map;
using Eldritch.Core;
using Eldritch.Core.Entities;
using Eldritch.Core.Components;
using Eldritch.Core.Rendering;

namespace Eldritch.Core.Tests
{
    public class RenderEngineTests
    {
        [Fact]
        public void Render_MapAndEntity_AppearInBuffer()
        {
            var map = new Eldritch.Core.Map.Map(3,3);
            // make center floor
            map.Set(1,1, TileType.Floor);

            var mgr = new EntityManager();
            var e = mgr.CreateEntity();
            e.AddComponent(new PositionComponent(1,1));

            var buf = AsciiRenderer.Render(map, mgr);

            Assert.Equal('#', buf[0,0]); // default wall
            // renderer places entity on top, so expect 'E' at center
            Assert.Equal('E', buf[1,1]);
        }
    }
}
