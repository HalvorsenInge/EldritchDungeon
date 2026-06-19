using Xunit;
using Eldritch.Core;
using Eldritch.Core.Components;

namespace Eldritch.Core.Tests
{
    public class EntityManagerTests
    {
        [Fact]
        public void CreateEntity_ShouldAssignUniqueIds()
        {
            var mgr = new EntityManager();
            var a = mgr.CreateEntity();
            var b = mgr.CreateEntity();
            Assert.NotEqual(a.Id, b.Id);
            Assert.Equal(2, mgr.Count);
        }

        [Fact]
        public void AddAndQueryComponent_ShouldReturnEntity()
        {
            var mgr = new EntityManager();
            var e = mgr.CreateEntity();
            e.AddComponent(new PositionComponent(3,4));

            var query = mgr.QueryByComponent<PositionComponent>();
            Assert.Contains(e, query);
        }

        [Fact]
        public void RemoveEntity_ShouldDecreaseCount()
        {
            var mgr = new EntityManager();
            var e = mgr.CreateEntity();
            Assert.Equal(1, mgr.Count);
            mgr.RemoveEntity(e.Id);
            Assert.Equal(0, mgr.Count);
        }
    }
}