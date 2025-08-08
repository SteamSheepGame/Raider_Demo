using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    public class FactoryService : IFactoryService
    {
        private readonly Dictionary<Type, IFactory> _factories = new();

        public void Register<T>(IFactory factory) where T : IEntity
        {
            _factories[typeof(T)] = factory;
        }

        public void Deregister<T>() where T : IEntity
        {
            if(_factories.ContainsKey(typeof(T)))
                _factories.Remove(typeof(T));
        }

        public IInstance Create(IEntity entity)
        {
            var type = entity.GetType();
            if (_factories.TryGetValue(type, out var factory))
            {
                return factory.Create(entity);
            }
            Debug.LogWarning($"No factory registered for {type.Name}");
            return null;
        }
    }
}