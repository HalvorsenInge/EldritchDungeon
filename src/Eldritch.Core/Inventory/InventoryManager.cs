using System.Collections.Generic;
using System.Linq;

namespace Eldritch.Core.Inventory
{
    public class InventoryManager
    {
        private readonly List<Item> _items = new();
        public IReadOnlyList<Item> Items => _items;

        public void Add(Item item)
        {
            _items.Add(item);
        }

        public bool Remove(Item item) => _items.Remove(item);

        public Item? FindByName(string name) => _items.FirstOrDefault(i => i.Name == name);

        public int Count => _items.Count;
    }
}