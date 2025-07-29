using UnityEngine;

namespace Demo.Core
{
    public interface IView
    {
        //void Initialize();

        void ShowView(string viewName);

        void HideView(string viewName);

        void ShowView<T>() where T : Component;
    }
}
