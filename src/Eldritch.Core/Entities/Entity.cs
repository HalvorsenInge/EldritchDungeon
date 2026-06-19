using System;
using System.Collections.Generic;

namespace Eldritch.Core.Entities
{
    public class Entity
    {
        public int Id { get; }
        private readonly Dictionary<Type, object> _components = new();

        internal Entity(int id) => Id = id;

        public void AddComponent<T>(T component) where T : class
        {
            _components[typeof(T)] = component ?? throw new ArgumentNullException(nameof(component));
        }

        public bool HasComponent<T>() where T : class => _components.ContainsKey(typeof(T));

        public T? GetComponent<T>() where T : class => _components.TryGetValue(typeof(T), out var c) ? c as T : null;

        public bool RemoveComponent<T>() where T : class => _components.Remove(typeof(T));
    }
}