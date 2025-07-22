namespace Demo.Core
{
    public interface ICardView
    {
        IEntity _entity { get; }
        public bool IsDraggable { get; set; }
        public bool IsSelected { get; set; }

        public void bind(IEntity entity);
    }
}