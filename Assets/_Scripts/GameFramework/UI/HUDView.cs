using UnityEngine;
using UnityEngine.UI;

namespace Demo.Core
{
    public class HUDView : View
    {
        [SerializeField] private Button QuestboardButton;
        protected override void Awake()
        {
            base.Awake();
            Debug.Log("[HUDView] Initialized");

            QuestboardButton.onClick.AddListener(OpenQuestboard);
        }

        void OpenQuestboard()
        {
            UIManager.Instance.SpawnPopup("Questboard");
        }
    }
}
