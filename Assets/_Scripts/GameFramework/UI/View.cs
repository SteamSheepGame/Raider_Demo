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
        protected UnityEngine.Canvas canvas;

        [SerializeField, TitleGroup("View Settings")]
        protected bool enableLazyLoad = true;

        [SerializeField, TitleGroup("View Settings")]
        protected string resourceFolder = "UI/Views";

        [SerializeField, TitleGroup("Panels (Optional Prebound)")]
        protected List<GameObject> preboundPanels = new();

        public bool IsDestroyOnLoad = false;
        

        protected readonly Dictionary<string, GameObject> panels = new();
        
        public string ViewName => canvas.name;
        public bool IsVisible => gameObject.activeInHierarchy;
        
        public RectTransform Rect
        {
            get
            {
                if (_rect == null && canvas != null)
                    _rect = canvas.GetComponent<RectTransform>();
                return _rect;
            }
        }
        
        private RectTransform _rect;
        
        protected virtual void Awake()
        {
            panels.Clear();

            foreach (var go in preboundPanels)
            {
                if (go == null) continue;
                if (!panels.ContainsKey(go.name))
                    panels.Add(go.name, go);
                else
                    Debug.LogWarning($"[View:{ViewName}] Duplicate panel name '{go.name}'.");
            }
            
            // 存储Rect
            if (_rect == null && canvas != null)
            {
                _rect = canvas.GetComponent<RectTransform>();
            }

            if (IsDestroyOnLoad == false)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            
        }
        

		protected virtual RectTransform GetRect()
        {
            return canvas.GetComponent<RectTransform>();
        }

        public virtual void Show(string panelName, bool exclusive = false, bool bringToFront = true)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogWarning($"[View:{ViewName}] Show called with empty panelName.");
                return;
            }

            if (!TryGetPanel(panelName, out var panel))
            {
                Debug.LogError($"[View:{ViewName}] Panel '{panelName}' not found or failed to load.");
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
                Debug.LogWarning($"[View:{ViewName}] BringToFront: '{panelName}' not found.");
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

                Debug.LogWarning($"[View:{ViewName}] Could not load prefab at '{path}'.");
            }

            panel = null;
            return false;
        }

        /// <summary>
        /// Get Canvas
        /// </summary>
        public virtual Canvas GetCanvas(){
            return canvas;
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
