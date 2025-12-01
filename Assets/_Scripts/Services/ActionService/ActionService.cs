using UnityEngine;
using System.Collections.Generic;
namespace Demo.Core
{
    public class ActionService: IActionService
    {
        private readonly Dictionary<string, IActionHandler> _handlers 
            = new Dictionary<string, IActionHandler>();

        public ActionService()
        {
            _handlers["OpenPopup"] = new OpenPopupAction();
            _handlers["Work"]      = new WorkAction();
            _handlers["AddCard"] = new AddCardAction();
        }
        public void ExecuteAction(ActionBinding action)
        {
            if (_handlers.TryGetValue(action.Id, out var handler))
            {
                handler.Execute(action);
            }
            else
            {
                Debug.LogWarning($"Unknown action: {action.Id}");
            }
            
        }
    }
}