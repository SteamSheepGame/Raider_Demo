using TMPro;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
namespace Demo.Core
{
    public class QuestboardPopup: BasePopup
    {
        [TitleGroup("UI References")] 
        [SerializeField, Required] private TextMeshProUGUI Title;
        [SerializeField, Required] private TextMeshProUGUI QuestDescription;
        [SerializeField, Required] private RectTransform scrollAreaRect;
        [SerializeField, Required] private GameObject questPrefab;
        
        private Dictionary<string, QuestSummary> QuestMap = new Dictionary<string, QuestSummary>();
        public override void Bind(IEntity entity)
        {
            base.Bind(entity);
            QuestboardPopupEntity questboardEntity = entity as QuestboardPopupEntity;

            if (questboardEntity == null)
            {
                Debug.LogError("Questboard Bind error");
            }
            
            Title.text = questboardEntity.Title;

            foreach (QuestSummary quest in questboardEntity.Quests)
            {
                QuestMap.Add(quest.Id, quest);
                AddQuest(quest);
            }
        }

        public void AddQuest(QuestSummary quest)
        {
            var questUI = Instantiate(questPrefab, scrollAreaRect);
            questUI.SetActive(true);

            QuestSummaryUI summary = questUI.GetComponent<QuestSummaryUI>();
            summary.Bind(quest);
            summary.OnSelected += () =>
            {
                QuestDescription.text = quest.Description;
            };
        }
    }
}