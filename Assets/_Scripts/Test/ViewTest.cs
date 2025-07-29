using UnityEngine;

using UnityEngine.UI;

namespace Demo.Core
{

    public class ViewTest : MonoBehaviour
    {
        [SerializeField] private Button _next;

        private void Start()
        {
            if (_next != null)
            {
                _next.onClick.AddListener(() =>
                {
                    View.Instance.ShowView("Test 2");
                    View.Instance.HideView("Test 1");
                });
                

            }
        }


    }
}