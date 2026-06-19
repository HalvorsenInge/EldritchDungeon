using System;

namespace Eldritch.Core.Inventory
{
    public record Item(Guid Id, string Name, string Type, int Charges = 0)
    {
        public Item(string name, string type, int charges = 0) : this(Guid.NewGuid(), name, type, charges) {}
    }
}