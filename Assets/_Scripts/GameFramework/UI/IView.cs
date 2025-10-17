using UnityEngine; 
namespace Demo.Core
{
    public interface IView
    {
        string ViewName { get; }
        bool IsVisible { get; }
		RectTransform Rect { get; }
        void Show(string panelName, bool exclusive = false, bool bringToFront = true);
        void Hide(string panelName = null);
        void BringToFront(string panelName);
        bool TryGetPanel(string panelName, out UnityEngine.GameObject panel);
        UnityEngine.Canvas GetCanvas();
    }
}
