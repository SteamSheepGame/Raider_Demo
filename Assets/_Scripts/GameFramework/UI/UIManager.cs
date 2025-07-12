using UnityEngine;

using System.Collections.Generic;

namespace Demo.Core
{
    public class UIManager : Singleton<UIManager>
    {
       private static UIManager _instance; 

       [SerializeField] private View startingview;

       ///List of Views in scene
       [SerializeField] private View[] views; 

       private View currentview;

       ///Happen Once
       private readonly Stack<View> History = new Stack<View>();  

       protected override void Awake() => _instance = this;

        /// <summary>
        /// Search View of the specified type
        /// </summary>
        public static T GetViews<T>() where T : View
        {
            for (int i = 0; i < _instance.views.Length; i++)
            {
                if(_instance.views[i] is T tView)
               {
                    return tView;
               }
            }
            return null;
        }

        /// <summary>
        ///Display View
        /// </summary>
        /// <param name="remember"></param>
        public static void Show<T>(bool remember = true) where T:View
        {
            for (int i = 0; i < _instance.views.Length; i++)
            {
                if (_instance.views[i] is T)
               {
                    if (_instance.currentview != null)
                    {
                        if(remember)
                        {
                            _instance.History.Push(_instance.currentview);
                        }

                        _instance.currentview.Hide();
                    }

                    _instance.views[i].Show();

                    _instance.currentview = _instance.views[i];

               }
            }
        }
        /// <summary>
        ///Display View
        /// </summary>
        /// <param name="remember"></param>
        /// <param name="remember"></param>
        public static void Show(View view, bool remember = true)
        {
            if (_instance.currentview != null)
            {
                if(remember)
                {
                    _instance.History.Push(_instance.currentview);
                }

                _instance.currentview.Hide();
            }

            view.Show();

            _instance.currentview = view;
        }
        /// <summary>
        ///Return to previous view
        /// </summary>
        public static void ShowLast()
        {
            if(_instance.History.Count != 0)
            {
                Show(_instance.History.Pop(), false);
            }
        }

        private void Start()
        {
            for (int i = 0; i < views.Length; i++)
            {
                views[i].Initialize();

                views[i].Hide();
            }

            if (startingview != null)
            {
                Show(startingview, true);
            }
        }
    }
}
