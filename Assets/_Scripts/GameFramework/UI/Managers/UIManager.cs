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
        
        [SerializeField, Required, TitleGroup("UI Reference")]
        private GameObject locationDeck;
        // Might be useful in the future to group multiple popups
        // private RectTransform popupParent;
        
        // Dictionary: viewName -> Canvas
        private readonly Dictionary<string, IView> viewLookup = new();
        
        private LocationDeckUI _locationDeckUI;

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
        
        public LocationDeckUI GetLocationDeckUI()
        {
            if (_locationDeckUI == null)
            {
                if (locationDeck != null)
                {
                    _locationDeckUI = locationDeck.GetComponent<LocationDeckUI>();
                }
            }

            return _locationDeckUI;
        }
        
        /// <summary>
        /// 生成Popup，json相关功能
        /// </summary>
        /// <param name="entity"></param>
        public void SpawnPopup(string Id)
        {
            IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
            IEntity entity = storeService.GetEntity(Id);
            if (entity == null)
            {
                Debug.LogError("[UIManager] Entity not found.");
            }

            guiView.TryGetPanel(entity.Id, out var panel);
            // 如果生成过，showPopup
            if (panel != null)
            {
                guiView.ShowPopup(entity.Id, panel);
            }
            else
            {
                // 生成新popup
                IFactoryService factoryService = ServiceProvider.Instance.GetService<IFactoryService>();
                IPopup popup = factoryService.Create(entity) as IPopup;
                // popup.Rect.SetParent(popupParent);
                if (popup is SerializedMonoBehaviour mb)
                {
                    guiView.AddPopup(mb.gameObject, entity.Id);
                }     
            }
           
        }

        public void CloseTopPopup() => guiView?.CloseTopPopup();
        public void ClosePopup(string panelName) => guiView?.ClosePopup(panelName);
        public void CloseAllPopups() => guiView?.CloseAllPopups();
        public bool AnyPopupOpen => guiView != null && guiView.HasOpenPopups;
    }
}
