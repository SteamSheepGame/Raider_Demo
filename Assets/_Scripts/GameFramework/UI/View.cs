using System.Collections.Generic;

using UnityEngine;


namespace Demo.Core
{

    public class View : Singleton<View>, IView
    {

        [SerializeField] private List<GameObject> viewObjects; //List of UI referenced at runtime

        [SerializeField] private bool enableLazyLoad = true;

        [SerializeField] private GameObject initialView;

        [SerializeField] private string resourceFolder = "UI/Views"; //path for lazy load ui prefabs


        /// <summary>
        /// “MainMenu”：MainMenuPanel GameObject
        /// </summary>
        private readonly Dictionary<string, GameObject> _views = new(); 

        protected override void Initialize()
        {
            base.Initialize();   /// Calls Singleton<T>.Initialize()
            _views.Clear();

            foreach (var view in viewObjects)
            {
                if (view != null)
                {
                    _views[view.name] = view;
                    if (view.name != initialView.name)
                    {
                        Debug.Log($"[View] name is {_views[view.name]} views.");
                        view.SetActive(false);
                    }
                    
                }
            }
            Debug.Log($"[View] Initialized with {_views.Count} views.");
        }

        /// <summary>
        /// Find and show view by name, if not found lazy-load it
        /// Activate View once found
        /// </summary>
        /// <param name="viewName"></param>
        public void ShowView(string viewName)
        {
            if (!_views.TryGetValue(viewName, out var view))
            {
                if (enableLazyLoad)
                {
                    var loaded = LoadAndRegisterView(viewName);
                    if (loaded != null)
                    {
                        view = loaded;
                    }
                    else
                    {
                        Debug.LogWarning($"[View] View '{viewName}' not found.");
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning($"[View] View '{viewName}' not found.");
                    return;
                }
            }

            view.SetActive(true);
        }

        /// <summary>
        /// “MainMenu”：MainMenuPanel GameObject
        /// </summary>
        /// <param name="viewName"></param>
        public void HideView(string viewName)
        {
            if (_views.TryGetValue(viewName, out var view))
            {
                view.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"[View] View '{viewName}' not found.");
            }
        }

        /// <summary>
        /// Generic overload of ShowView()
        /// </summary>
        /// <param name="viewName"></param>
        public void ShowView<T>() where T : Component
        {
            string typeName = typeof(T).Name;

            if (!_views.TryGetValue(typeName, out var view))
            {
                if (enableLazyLoad)
                {
                    view = LoadAndRegisterView(typeName);
                }

                if (view == null)
                {
                    Debug.LogWarning($"[View] View of type '{typeName}' not found.");
                    return;
                }
            }

            view.SetActive(true);
        }
        /// <summary>
        /// Lazy Loading View
        /// </summary>
        /// <param name="viewName"></param>
        private GameObject LoadAndRegisterView(string viewName)
        {
            string path = string.IsNullOrEmpty(resourceFolder)
                ? viewName  ///use viewName directly if empty
                : $"{resourceFolder}/{viewName}"; ///construct full path

            var prefab = Resources.Load<GameObject>(path);
            Debug.Log($"[Path] Current path at {path}");

            if (prefab == null)
            {
                Debug.LogWarning($"[View] Could not load prefab at {path}");
                return null;
            }

            Transform canvas = FindFirstObjectByType<Canvas>()?.transform ?? transform;  ///prefab parented under canvas
            var instance = Instantiate(prefab, canvas);
            instance.name = prefab.name; /// Remove (Clone)
            _views[instance.name] = instance;
            instance.SetActive(false); /// Hide until requested
            return instance;
        }
    }
}
