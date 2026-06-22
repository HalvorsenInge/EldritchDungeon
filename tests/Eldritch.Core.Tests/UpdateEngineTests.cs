using Xunit;
using Eldritch.Core;
using Eldritch.Core.Components;
using Eldritch.Core.Systems;
using Eldritch.Core.Input;
using Eldritch.Core.Map;

namespace Eldritch.Core.Tests
{
    public class UpdateEngineTests
    {
        [Fact]
        public void ResolveTurn_AppliesMovementIntent()
        {
            var mgr = new EntityManager();
            var e = mgr.CreateEntity();
            e.AddComponent(new PositionComponent(5,5));
            e.AddComponent(new IntentComponent(Commands.MoveUp));

            var engine = new UpdateEngine();
            engine.ResolveTurn(mgr);

            var pos = e.GetComponent<PositionComponent>();
            Assert.Equal(5, pos.X); // MoveUp decrements Y; X remains
            Assert.Equal(4, pos.Y);
        }

        [Fact]
        public void ResolveTurn_ClearsIntent()
        {
            var mgr = new EntityManager();
            var e = mgr.CreateEntity();
            e.AddComponent(new PositionComponent(0,0));
            e.AddComponent(new IntentComponent(Commands.Wait));

            var engine = new UpdateEngine();
            engine.ResolveTurn(mgr);

            Assert.False(e.HasComponent<IntentComponent>());
        }

        [Fact]
        public void ResolveTurn_BlocksWallMovement()
        {
            var mgr = new EntityManager();
            var e = mgr.CreateEntity();
            e.AddComponent(new PositionComponent(5,5));
            e.AddComponent(new IntentComponent(Commands.MoveUp));

            // default map tiles are Wall; movement into wall should be blocked
            var map = new Eldritch.Core.Map.Map(10,10);

            var engine = new UpdateEngine();
            engine.ResolveTurn(mgr, map);

            var pos = e.GetComponent<PositionComponent>();
            Assert.Equal(5, pos.X);
            Assert.Equal(5, pos.Y); // blocked by wall at (5,4)
        }

        [Fact]
        public void ResolveTurn_AllowsMovementOnFloor()
        {
            var mgr = new EntityManager();
            var e = mgr.CreateEntity();
            e.AddComponent(new PositionComponent(5,5));
            e.AddComponent(new IntentComponent(Commands.MoveUp));

            var map = new Eldritch.Core.Map.Map(10,10);
            map.Set(5,4, TileType.Floor);

            var engine = new UpdateEngine();
            engine.ResolveTurn(mgr, map);

            var pos = e.GetComponent<PositionComponent>();
            Assert.Equal(5, pos.X);
            Assert.Equal(4, pos.Y); // moved into floor tile
        }
    }
}
