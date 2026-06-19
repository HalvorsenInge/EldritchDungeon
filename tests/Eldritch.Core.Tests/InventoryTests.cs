using Xunit;
using Eldritch.Core.Inventory;

namespace Eldritch.Core.Tests
{
    public class InventoryTests
    {
        [Fact]
        public void AddAndFindByName_Works()
        {
            var inv = new InventoryManager();
            var potion = new Item("Healing Potion", "consumable", 1);
            inv.Add(potion);
            Assert.Equal(1, inv.Count);
            var found = inv.FindByName("Healing Potion");
            Assert.NotNull(found);
            Assert.Equal(potion.Id, found.Id);
        }

        [Fact]
        public void Remove_Works()
        {
            var inv = new InventoryManager();
            var item = new Item("Torch", "utility");
            inv.Add(item);
            Assert.True(inv.Remove(item));
            Assert.Equal(0, inv.Count);
        }
    }
}