namespace Demo.Core
{
    public interface ICard : IInstance
    {
        IEntity Entity { get; }
		// Marked for change
        public bool IsDraggable { get; set; }
        public bool IsSelected { get; set; }

        public void Bind(IEntity entity);
    }
}