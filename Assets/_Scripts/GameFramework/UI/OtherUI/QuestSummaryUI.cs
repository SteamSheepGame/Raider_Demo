using TMPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Demo.Core
{
    public class QuestSummaryUI: SerializedMonoBehaviour
    {
        [TitleGroup("UI References")]
        [SerializeField] private TextMeshProUGUI questTitle;  
        [SerializeField] private Button questButton;
        
        public event Action OnSelected;
        private QuestSummary quest;
        
        public void Bind(QuestSummary questData)
        {
            quest = questData;
            questTitle.text = quest.Title;
            questButton.onClick.AddListener(HandleClick);
        }
        
        private void HandleClick()
        {
            OnSelected?.Invoke();
        }
    }
}