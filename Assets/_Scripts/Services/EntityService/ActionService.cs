using UnityEngine;

namespace Demo.Core
{
    public class ActionService: IActionService
    {
        public void ExecuteAction(ActionBinding action)
        {
            switch (action.Id)
            {
                case "OpenPopup":
                    UIManager.Instance.SpawnPopup(action.PopupId);
                    break;
                default:
                    Debug.LogWarning($"Unknown action: {action.Id}");
                    break;
            }
        }
    }
}