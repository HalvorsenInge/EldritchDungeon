using System;
using System.Collections.Generic;
using System.Linq;
using Eldritch.Core.Entities;

namespace Eldritch.Core
{
    public class EntityManager
    {
        private int _nextId = 1;
        private readonly Dictionary<int, Entity> _entities = new();

        public Entity CreateEntity()
        {
            var e = new Entity(_nextId++);
            _entities[e.Id] = e;
            return e;
        }

        public bool RemoveEntity(int id) => _entities.Remove(id);

        public Entity? GetEntity(int id) => _entities.TryGetValue(id, out var e) ? e : null;

        public IEnumerable<Entity> QueryByComponent<T>() where T : class
        {
            return _entities.Values.Where(e => e.HasComponent<T>());
        }

        public int Count => _entities.Count;
    }
}