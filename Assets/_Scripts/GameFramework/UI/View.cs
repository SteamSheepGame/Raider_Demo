using UnityEngine;

using System.Collections.Generic;

namespace Demo.Core
{
    public class View : Singleton<View>, IView
    {

       [SerializeField] private IView startingview;

       //List of Views in scene
       [SerializeField] private IView[] views; 

       private IView currentview;

        public void Show()
        {
            gameObject.SetActive(true);
            Debug.Log("View shown.");
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            Debug.Log("View hidden.");
        }

        public void InitializeView()
        {
            for (int i = 0; i < views.Length; i++)
            {
                views[i].InitializeView();

                views[i].Hide();
            }

            if (startingview != null)
            {
                Show(startingview, true);
            }
        }

        /// <summary>
        /// Search View of the specified type
        /// </summary>
        public T GetViews<T>() where T : IView
        {
            for (int i = 0; i < this.views.Length; i++)
            {
                if(this.views[i] is T tView)
               {
                    return tView;
               }
            }
            return default(T);
        }

        /// <summary>
        ///Display View
        /// </summary>
        /// <param name="remember"></param>
        public void Show<T>(bool remember = true) where T:IView
        {
            for (int i = 0; i < this.views.Length; i++)
            {
                if (this.views[i] is T)
               {
                    if (this.currentview != null)
                    {

                        this.currentview.Hide();
                    }

                    this.views[i].Show();

                    this.currentview = this.views[i];

               }
            }
        }
        /// <summary>
        ///Display View
        /// </summary>
        /// <param name="remember"></param>
        /// <param name="remember"></param>
        public void Show(IView view, bool remember = true)
        {
            if (this.currentview != null)
            {
                this.currentview.Hide();
            }

            view.Show();

            this.currentview = view;
        }


        public void UpdateView()
        {
            Debug.Log("View updated.");
        }

    }
}
