namespace Demo.Core
{
    public interface IActionHandler
    {
        void Execute(ActionBinding binding);
    }
}