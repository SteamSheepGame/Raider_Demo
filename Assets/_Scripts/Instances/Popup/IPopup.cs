using System.ComponentModel.Design;
using UnityEngine;
namespace Demo.Core
{
    public interface IPopup: IInstance
    { 
        RectTransform Rect { get; }
        void OnPopupClosed();
        
        void SetWorkTask(WorkTask task);
        void StartWork();
    }
}