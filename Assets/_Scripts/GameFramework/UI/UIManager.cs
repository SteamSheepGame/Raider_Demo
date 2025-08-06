using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    /// <summary>
    /// Controls all canvases and routes view requests to the correct View manager.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private List<GameObject> canvasObjects; // Canvases in scene
        [SerializeField] private GameObject defaultCanvas;

        private readonly Dictionary<string, GameObject> _canvasDict = new();

        protected override void Initialize()
        {
            base.Initialize();
            _canvasDict.Clear();

            foreach (var canvas in canvasObjects)
            {
                if (canvas != null)
                {
                    _canvasDict[canvas.name] = canvas;

                    if (defaultCanvas == null || canvas.name != defaultCanvas.name)
                        canvas.SetActive(false);
                }
            }

            Debug.Log($"[UIManager] Initialized with {_canvasDict.Count} canvases.");
        }

        public void ShowCanvas(string canvasName)
        {
            if (_canvasDict.TryGetValue(canvasName, out var canvas))
            {
                canvas.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"[UIManager] Canvas '{canvasName}' not found.");
            }
        }

        public void HideCanvas(string canvasName)
        {
            if (_canvasDict.TryGetValue(canvasName, out var canvas))
            {
                canvas.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"[UIManager] Canvas '{canvasName}' not found.");
            }
        }

        public View GetViewManager(string canvasName)
        {
            if (_canvasDict.TryGetValue(canvasName, out var canvas))
                return canvas.GetComponent<View>();

            Debug.LogWarning($"[UIManager] Canvas '{canvasName}' not found.");
            return null;
        }

        public void ShowViewOnCanvas(string canvasName, string viewName)
        {
            var viewManager = GetViewManager(canvasName);
            if (viewManager != null)
            {
                ShowCanvas(canvasName);
                viewManager.ShowView(viewName);
            }
        }

        public void ShowViewOnCanvas<T>(string canvasName) where T : Component
        {
            var viewManager = GetViewManager(canvasName);
            if (viewManager != null)
            {
                ShowCanvas(canvasName);
                viewManager.ShowView<T>();
            }
        }

        public void PushPopup(string viewName)
        {
            var popupManager = GetViewManager("PopupCanvas") as PopupView;
            if (popupManager != null)
            {
                ShowCanvas("PopupCanvas");
                popupManager.PushView(viewName);
            }
        }

        public void PopPopup()
        {
            var popupManager = GetViewManager("PopupCanvas") as PopupView;
            popupManager?.PopView();
        }
    }
}
