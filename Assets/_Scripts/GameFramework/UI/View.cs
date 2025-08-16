using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Demo.Core
{
    [RequireComponent(typeof(Canvas))]

    public abstract class View : SerializedMonoBehaviour, IView
    {
        [SerializeField, TitleGroup("View Settings")]
        protected string viewName = "UnnamedView";

        [SerializeField, TitleGroup("View Settings")]
        protected bool enableLazyLoad = true;

        [SerializeField, TitleGroup("View Settings")]
        protected string resourceFolder = "UI/Views";

        [SerializeField, TitleGroup("Panels (Optional Prebound)")]
        protected List<GameObject> preboundPanels = new();

        protected readonly Dictionary<string, GameObject> panels = new();

        public string ViewName => viewName;
        public bool IsVisible => gameObject.activeInHierarchy;

        protected virtual void Awake()
        {
            panels.Clear();

            foreach (var go in preboundPanels)
            {
                if (go == null) continue;
                if (!panels.ContainsKey(go.name))
                    panels.Add(go.name, go);
                else
                    Debug.LogWarning($"[View:{viewName}] Duplicate panel name '{go.name}'.");
            }
        }

        public virtual void Show(string panelName, bool exclusive = false, bool bringToFront = true)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogWarning($"[View:{viewName}] Show called with empty panelName.");
                return;
            }

            if (!TryGetPanel(panelName, out var panel))
            {
                Debug.LogError($"[View:{viewName}] Panel '{panelName}' not found or failed to load.");
                return;
            }

            if (exclusive) HideAllExcept(panelName);

            panel.SetActive(true);

            if (bringToFront)
                panel.transform.SetAsLastSibling();
        }

        public virtual void Hide(string panelName = null)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                HideAll();
                return;
            }

            if (panels.TryGetValue(panelName, out var panel) && panel != null)
                panel.SetActive(false);
        }

        /// <summary>
        /// Bring specific panel to the front
        /// </summary>
        public virtual void BringToFront(string panelName)
        {
            if (panels.TryGetValue(panelName, out var panel) && panel != null)
                panel.transform.SetAsLastSibling();
            else
                Debug.LogWarning($"[View:{viewName}] BringToFront: '{panelName}' not found.");
        }

        /// <summary>
        /// Lazy load panel from resource/UI/Views
        /// </summary>
        public virtual bool TryGetPanel(string panelName, out GameObject panel)
        {
            if (panels.TryGetValue(panelName, out panel) && panel != null)
                return true;

            if (enableLazyLoad)
            {
                var path = string.IsNullOrEmpty(resourceFolder)
                    ? panelName
                    : $"{resourceFolder}/{panelName}";

                var prefab = Resources.Load<GameObject>(path);
                if (prefab != null)
                {
                    panel = Instantiate(prefab, transform);
                    panel.name = prefab.name;
                    panels[panel.name] = panel;
                    panel.SetActive(false);
                    return true;
                }

                Debug.LogWarning($"[View:{viewName}] Could not load prefab at '{path}'.");
            }

            panel = null;
            return false;
        }

        protected void HideAll()
        {
            foreach (var kv in panels)
            {
                if (kv.Value != null)
                    kv.Value.SetActive(false);
            }
        }

        protected void HideAllExcept(string keepPanelName)
        {
            foreach (var kv in panels)
            {
                if (kv.Value == null) continue;
                if (kv.Key == keepPanelName) continue;
                kv.Value.SetActive(false);
            }
        }
    }
}
