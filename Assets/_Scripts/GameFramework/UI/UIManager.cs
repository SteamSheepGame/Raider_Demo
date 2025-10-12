using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

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

        // Dictionary: viewName -> Canvas
        private readonly Dictionary<string, IView> viewLookup = new();

        // 增加public getter
        public HUDView HUDView => hudView;
        public GUIView GUIView => guiView;
        protected override void Initialize()
        {
            base.Initialize();

            // cache both HUD and GUI
            CacheView(hudView, hudSortingOrder);
            CacheView(guiView, guiSortingOrder);
        }

        private void CacheView(View view, int order)
        {
            if (view == null) return;

            var canvas = view.GetComponent<Canvas>();
            if (canvas == null) return;

            canvas.overrideSorting = true;
            canvas.sortingOrder = order;

            viewLookup[view.ViewName] = view;
        }

        /// <summary>
        /// canvas sorting
        /// </summary>
        public void SetCanvasOrder(string viewName, int newOrder)
        {
            if (!viewLookup.TryGetValue(viewName, out var iview))
            {
                Debug.LogWarning($"[UIManager] No canvas cached with name '{viewName}'.");
                return;
            }
            iview.GetCanvas().sortingOrder = newOrder;
        }

        public int GetCanvasOrder(View view, string viewName)
        {
            return viewLookup.TryGetValue(viewName, out var iview)
                ? iview.GetCanvas().sortingOrder
                : -1;
        }

        /// <summary>
        /// Brings one canvas in front of all others
        /// </summary>
        public void FocusCanvas(View view,string focusViewName)
        {
            int topOrder = 2;
            int bottomOrder = 1;

            foreach (var kv in viewLookup)
            {
                if (kv.Key == focusViewName)
                    kv.Value.GetCanvas().sortingOrder = topOrder;
                else
                    kv.Value.GetCanvas().sortingOrder = bottomOrder;
            }
        }

        private IView GetView(string viewName)
        {
            if (viewLookup.TryGetValue(viewName, out var v))
                return v;

            Debug.LogError($"[UIManager] No IView cached for '{viewName}'.");
            return null;
        }

        public void ShowViewOnCanvas(string canvasName, string panelName, bool exclusive = false, bool bringToFront = true)
            => GetView(canvasName)?.Show(panelName, exclusive, bringToFront);

        public void HideCanvas(string canvasName, string panelName = null)
            => GetView(canvasName)?.Hide(panelName);

        public void BringPanelToFront(string canvasName, string panelName)
            => GetView(canvasName)?.BringToFront(panelName);

        public void ShowPopup(string panelName, bool modal = true)
        {
            if (guiView == null)
            {
                Debug.LogWarning("[UIManager] ShowPopup called but guiView is null.");
                return;
            }
            guiView.ShowPopup(panelName, modal);
        }

        public void CloseTopPopup() => guiView?.CloseTopPopup();
        public void ClosePopup(string panelName) => guiView?.ClosePopup(panelName);
        public void CloseAllPopups() => guiView?.CloseAllPopups();
        public bool AnyPopupOpen => guiView != null && guiView.HasOpenPopups;
    }
}
