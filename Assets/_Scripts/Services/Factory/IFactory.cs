namespace Demo.Core
{
    public interface IFactory
    {
        IInstance Create(IEntity entity);
    }
}