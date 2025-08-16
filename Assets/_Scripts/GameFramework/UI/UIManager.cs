using Sirenix.OdinInspector;
using UnityEngine;

namespace Demo.Core
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField, Required, TitleGroup("Canvases")]
        private HUDView hudView;

        [SerializeField, Required, TitleGroup("Canvases")]
        private GUIView guiView;

        [SerializeField, TitleGroup("Sorting")]
        private int hudSortingOrder = 0;

        [SerializeField, TitleGroup("Sorting")]
        private int guiSortingOrder = 1;

        protected override void Initialize()
        {
            base.Initialize();
            ApplySorting(hudView, hudSortingOrder);
            ApplySorting(guiView, guiSortingOrder);
        }

        private void ApplySorting(View view, int order)
        {
            if (view == null) return;
            var c = view.GetComponent<Canvas>();
            if (c != null)
            {
                c.overrideSorting = true;
                c.sortingOrder = order;
            }
        }

        private IView GetView(string canvasName)
        {
            if (hudView != null && (hudView.ViewName == canvasName || hudView.name == canvasName)) return hudView;
            if (guiView != null && (guiView.ViewName == canvasName || guiView.name == canvasName)) return guiView;

            Debug.LogError($"[UIManager] No canvas/view found named '{canvasName}'.");
            return null;
        }

        // ---- Generic show/hide API ----
        public void ShowOnHUD(string panelName, bool exclusive = false, bool top = true)
            => hudView?.Show(panelName, exclusive, top);

        public void ShowOnGUI(string panelName, bool exclusive = false, bool top = true)
            => guiView?.Show(panelName, exclusive, top);

        public void HideHUD(string panelName = null) => hudView?.Hide(panelName);
        public void HideGUI(string panelName = null) => guiView?.Hide(panelName);

        public void ShowViewOnCanvas(string canvasName, string panelName, bool exclusive = false, bool bringToFront = true)
            => GetView(canvasName)?.Show(panelName, exclusive, bringToFront);

        public void HideCanvas(string canvasName, string panelName = null)
            => GetView(canvasName)?.Hide(panelName);

        public void BringPanelToFront(string canvasName, string panelName)
            => GetView(canvasName)?.BringToFront(panelName);

        public void FocusCanvas(string focusCanvasName)
        {
            var hudOrder = (focusCanvasName == hudView?.ViewName || focusCanvasName == hudView?.name) ? 2 : 1;
            var guiOrder = (focusCanvasName == guiView?.ViewName || focusCanvasName == guiView?.name) ? 2 : 1;
            ApplySorting(hudView, hudOrder);
            ApplySorting(guiView, guiOrder);
        }

        // ---- Popup helpers (directly in UIManager) ----
        public void ShowPopup(string panelName, bool modal = true)
        {
            if (guiView == null)
            {
                Debug.LogWarning("[UIManager] ShowPopup called but guiView is null.");
                return;
            }
            guiView.ShowPopup(panelName, modal);
        }

        public void CloseTopPopup()
        {
            if (guiView == null) return;
            guiView.CloseTopPopup();
        }

        public void ClosePopup(string panelName)
        {
            if (guiView == null) return;
            guiView.ClosePopup(panelName);
        }

        public void CloseAllPopups()
        {
            if (guiView == null) return;
            guiView.CloseAllPopups();
        }

        public bool AnyPopupOpen => guiView != null && guiView.HasOpenPopups;
    }
}
