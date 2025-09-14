namespace Demo.Core
{
    public interface IFactoryService : IService
    {
        public void Register<T>(IFactory factory) where T : IEntity;
        public void Deregister<T>() where T : IEntity;
        public IInstance Create(IEntity entity);
    }
}