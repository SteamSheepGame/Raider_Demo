using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    public class View : MonoBehaviour, IView
    {
        [SerializeField] private List<GameObject> viewObjects; // Panels inside this canvas
        [SerializeField] private bool enableLazyLoad = true;
        [SerializeField] private GameObject initialView;
        [SerializeField] private string resourceFolder = "UI/Views"; 

        private readonly Dictionary<string, GameObject> _views = new();

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _views.Clear();

            foreach (var view in viewObjects)
            {
                if (view != null)
                {
                    _views[view.name] = view;

                    if (initialView == null || view.name != initialView.name)
                        view.SetActive(false);
                }
            }

            Debug.Log($"[View] Initialized with {_views.Count} views on canvas '{gameObject.name}'.");
        }

         /// <summary>
        /// Show a view. If not found and lazy load enabled, load it from Resources.
        /// Does NOT hide other views.
        /// </summary>
        public void ShowView(string viewName)
        {
            if (!_views.TryGetValue(viewName, out var view))
            {
                if (enableLazyLoad)
                    view = LoadAndRegisterView(viewName);

                if (view == null)
                {
                    Debug.LogWarning($"[View] View '{viewName}' not found in canvas '{name}'.");
                    return;
                }
            }

            view.SetActive(true);
        }

        /// <summary>
        /// Hide a specific view. Will destroy if tagged "TempView".
        /// </summary>
        public void HideView(string viewName)
        {
            if (_views.TryGetValue(viewName, out var view))
            {
                view.SetActive(false);

                if (view.CompareTag("TempView"))
                {
                    _views.Remove(viewName);
                    Destroy(view);
                }
            }
            else
            {
                Debug.LogWarning($"[View] View '{viewName}' not found in canvas '{name}'.");
            }
        }

        /// <summary>
        /// Show a view by type name.
        /// </summary>
        public void ShowView<T>() where T : Component
        {
            string typeName = typeof(T).Name;

            if (!_views.TryGetValue(typeName, out var view))
            {
                if (enableLazyLoad)
                    view = LoadAndRegisterView(typeName);

                if (view == null)
                {
                    Debug.LogWarning($"[View] View of type '{typeName}' not found in canvas '{name}'.");
                    return;
                }
            }

            view.SetActive(true);
        }

        /// <summary>
        /// Hide all views in this canvas (useful for resetting UI).
        /// </summary>
        public void HideAllViews()
        {
            foreach (var view in _views.Values)
                view.SetActive(false);
        }

        private GameObject LoadAndRegisterView(string viewName)
        {
            string path = string.IsNullOrEmpty(resourceFolder)
                ? viewName
                : $"{resourceFolder}/{viewName}";

            var prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
            {
                Debug.LogWarning($"[View] Could not load prefab at {path}");
                return null;
            }

            var instance = Instantiate(prefab, transform);
            instance.name = prefab.name;
            _views[instance.name] = instance;
            instance.SetActive(false);

            return instance;
        }
    }
}
