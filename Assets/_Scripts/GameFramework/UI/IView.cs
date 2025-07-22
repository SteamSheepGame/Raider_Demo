using UnityEngine;

namespace Demo.Core
{
    public interface IView
    {
        void InitializeView();

        void Hide();

        void Show();

        void UpdateView();
    }
}
