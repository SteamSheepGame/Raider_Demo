using UnityEngine;

using UnityEngine.UI;

namespace Demo.Core
{

    public class UITest : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                UIManager.Instance.ShowViewOnCanvas("MainGameplayCanvas", "Verb");

            if (Input.GetKeyDown(KeyCode.Alpha2))
                UIManager.Instance.ShowViewOnCanvas("MainGameplayCanvas", "Lore");

            if (Input.GetKeyDown(KeyCode.Alpha3))
                UIManager.Instance.ShowViewOnCanvas("PopupCanvas", "DetailPopup");

            if (Input.GetKeyDown(KeyCode.Alpha4))
                UIManager.Instance.ShowViewOnCanvas("OverlayCanvas", "Notification");

            if (Input.GetKeyDown(KeyCode.C))
                UIManager.Instance.HideCanvas("PopupCanvas");
        }
    }
}