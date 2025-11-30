namespace Demo.Core
{
    public interface IActionService: IService
    {
        void ExecuteAction(ActionBinding action);
    }
}