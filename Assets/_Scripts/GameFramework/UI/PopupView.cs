using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    public class PopupView : View
    {
        private readonly Stack<GameObject> _popupStack = new();

        /// <summary>
        /// Push a popup on top of the stack (blocks others)
        /// </summary>
        public void PushView(string viewName)
        {
            var popup = GetOrLoadView(viewName);
            if (popup == null) return;

            Time.timeScale = 0;
            if (_popupStack.Count > 0)
                _popupStack.Peek().SetActive(false);

            popup.SetActive(true);
            _popupStack.Push(popup);
        }

        /// <summary>
        /// Pops the topmost popup
        /// </summary>
        public void PopView()
        {
            if (_popupStack.Count == 0) return;

            var topPopup = _popupStack.Pop();
            Time.timeScale = 1;
            Destroy(topPopup); // Or set inactive if reusing

            if (_popupStack.Count > 0)
                _popupStack.Peek().SetActive(true);
        }

        /// <summary>
        /// Clears all popups
        /// </summary>
        public void ClearAll()
        {
            while (_popupStack.Count > 0)
            {
                var popup = _popupStack.Pop();
                Destroy(popup);
            }
        }

        /// <summary>
        /// Helper to get or lazy-load a view
        /// </summary>
        private GameObject GetOrLoadView(string viewName)
        {
            // Use View's internal dictionary and lazy loading
            var field = typeof(View).GetField("_views", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var viewsDict = (Dictionary<string, GameObject>)field.GetValue(this);

            if (!viewsDict.TryGetValue(viewName, out var view))
            {
                var method = typeof(View).GetMethod("LoadAndRegisterView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                view = (GameObject)method.Invoke(this, new object[] { viewName });
            }

            return view;
        }
    }
}
