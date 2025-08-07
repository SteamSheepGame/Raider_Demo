using UnityEngine;
using System;
using System.Collections.Generic;

namespace Demo.Core
{
    public static class CardFactoryRegistry
    {
        // Key: Type of IEntity, Value: Associated Factory
        private static Dictionary<Type, ICardFactory> _factories = new();
        
        public static void Register<T>(ICardFactory factory) where T : IEntity
        {
            _factories[typeof(T)] = factory;
        }

        public static ICardView CreateCard(IEntity entity)
        {
            var type = entity.GetType();
            if (_factories.TryGetValue(type, out var factory))
            {
                return factory.CreateCard(entity);
            }

            Debug.LogWarning($"No factory registered for {type.Name}");
            return null;
        }
    }

}