namespace Demo.Core
{
    public class OpenPopupAction: BaseActionHandler
    {
        public override void Execute(ActionBinding action)
        {
            string popupId = GetParam(action, "PopupId");
            UIManager.Instance.SpawnPopup(popupId);
        }
    }
}