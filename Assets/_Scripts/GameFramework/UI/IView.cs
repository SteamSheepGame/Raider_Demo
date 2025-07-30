using UnityEngine;

namespace Demo.Core
{
    public interface IView
    {

        void ShowView(string viewName);

        void HideView(string viewName);

        void ShowView<T>() where T : Component;
    }
}
