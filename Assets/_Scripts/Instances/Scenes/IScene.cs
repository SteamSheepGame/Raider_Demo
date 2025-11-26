namespace Demo.Core
{
    public interface IScene: IInstance
    {
        IEntity Entity { get; }
        public void Bind(IEntity entity);
    }
}