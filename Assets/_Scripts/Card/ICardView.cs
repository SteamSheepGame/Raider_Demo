namespace Demo.Core
{
    public interface ICardView
    {
        IEntity Entity { get; }
		// Marked for change
        public bool IsDraggable { get; set; }
        public bool IsSelected { get; set; }

        public void bind(IEntity entity);
    }
}