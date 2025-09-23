using UnityEngine;

namespace Demo.Core
{
    public abstract class Factory<T> : IFactory where T :IInstance
    {
        protected readonly GameObject Prefab;

        protected Factory(GameObject prefab)
        {
            Prefab = prefab;
        }

        protected abstract T CreateInstance(IEntity entity);

        public IInstance Create(IEntity entity)
        {
            return CreateInstance(entity);
        }
    }
}