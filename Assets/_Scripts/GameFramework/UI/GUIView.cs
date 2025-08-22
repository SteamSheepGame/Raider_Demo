using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.Core
{
    public class GUIView : View
    {
        [TitleGroup("Popups")]
        [SerializeField] private RectTransform popupRoot;           // Optional: where popups live. If null, use this.transform
        [SerializeField] private GameObject dimmerBackdrop;         // Optional: a full-screen Image with RaycastTarget to block input
        [SerializeField] private bool enableEscToCloseTop = true;   // ESC closes top popup

        private readonly Stack<GameObject> popupStack = new();

        protected override void Awake()
        {
            base.Awake();
            if (popupRoot == null) popupRoot = (RectTransform)transform;
            if (dimmerBackdrop != null) dimmerBackdrop.SetActive(false);
        }

        private void Update()
        {
            if (!enableEscToCloseTop) return;
            if (Input.GetKeyDown(KeyCode.Escape)) //后续不能用这个input的写法
                CloseTopPopup();
        }

        /// <summary>
        /// Show a popup and push to the stack.
        /// </summary>
        public void ShowPopup(string panelName, bool modal = true)
        {
            if (!TryGetPanel(panelName, out var panel))
            {
                Debug.LogError($"[GUIView] ShowPopup: '{panelName}' could not be found/loaded.");
                return;
            }

            // Parent under popupRoot to keep all popups grouped
            if (panel.transform.parent != popupRoot)
                panel.transform.SetParent(popupRoot, worldPositionStays: false);

            // Activate and move to top
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();

            popupStack.Push(panel);
            UpdateBackdrop(modal);
        }

        /// <summary>
        /// Close the top-most popup if present.
        /// </summary>
        public void CloseTopPopup()
        {
            if (popupStack.Count == 0) return;

            var top = popupStack.Pop();
            if (top != null) top.SetActive(false);
            UpdateBackdrop(IsModalExpectedForTop());
        }

        /// <summary>
        /// Close a specific popup by name (if it exists in the stack).
        /// </summary>
        public void ClosePopup(string panelName)
        {
            if (string.IsNullOrEmpty(panelName)) return;

            // If top matches, pop quickly.
            if (popupStack.Count > 0 && popupStack.Peek() != null && popupStack.Peek().name == panelName)
            {
                CloseTopPopup();
                return;
            }

            // Otherwise rebuild stack without the target.
            var buffer = new Stack<GameObject>();
            GameObject target = null;

            while (popupStack.Count > 0)
            {
                var p = popupStack.Pop();
                if (p != null && p.name == panelName && target == null)
                {
                    target = p; // first match
                    continue;
                }
                buffer.Push(p);
            }
            while (buffer.Count > 0) popupStack.Push(buffer.Pop());

            if (target != null) target.SetActive(false);
            UpdateBackdrop(IsModalExpectedForTop());
        }

        /// <summary>
        /// Close all popups.
        /// </summary>
        public void CloseAllPopups()
        {
            while (popupStack.Count > 0)
            {
                var p = popupStack.Pop();
                if (p != null) p.SetActive(false);
            }
            UpdateBackdrop(false);
        }

        /// <summary>
        /// Bring an existing popup or panel to front and (if popup) fix the stack order.
        /// </summary>
        public override void BringToFront(string panelName)
        {
            if (!panels.TryGetValue(panelName, out var panel) || panel == null)
            {
                Debug.LogWarning($"[GUIView] BringToFront: '{panelName}' not found.");
                return;
            }

            panel.transform.SetAsLastSibling();

            // If it's in the popup stack, re-stack it to top
            if (popupStack.Count > 0)
            {
                var buffer = new Stack<GameObject>();
                bool found = false;

                while (popupStack.Count > 0)
                {
                    var p = popupStack.Pop();
                    if (!found && p == panel)
                    {
                        found = true;
                        continue;
                    }
                    buffer.Push(p);
                }
                while (buffer.Count > 0) popupStack.Push(buffer.Pop());
                if (found) popupStack.Push(panel);

                UpdateBackdrop(IsModalExpectedForTop());
            }
        }

        /// <summary>
        /// Convenience: open a non-stacked panel (uses base.Show).
        /// </summary>
        public void ShowNonPopup(string panelName, bool exclusive = false, bool bringToFront = true)
            => Show(panelName, exclusive, bringToFront);

        public bool HasOpenPopups => popupStack.Count > 0;

        private void UpdateBackdrop(bool shouldShow)
        {
            if (dimmerBackdrop == null)
                return;

            dimmerBackdrop.SetActive(shouldShow && popupStack.Count > 0);

            if (dimmerBackdrop.activeSelf)
            {
                // place backdrop just beneath the top-most popup
                dimmerBackdrop.transform.SetParent(popupRoot, false);
                dimmerBackdrop.transform.SetAsLastSibling();
                // then put the actual popup above it
                var top = popupStack.Peek();
                if (top != null) top.transform.SetAsLastSibling();
            }
        }

        // If you want to vary modal behavior per-popup, change this or pass a map.
        private bool IsModalExpectedForTop() => popupStack.Count > 0;
    }
}
