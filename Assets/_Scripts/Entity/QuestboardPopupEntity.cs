using System.Collections.Generic;
using System;
namespace Demo.Core
{
    [DataImportable("QuestboardPopup")]
    public class QuestboardPopupEntity: IEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<QuestSummary> Quests{ get; set; }
    }

    [Serializable]
    public class QuestSummary
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NextQuestId { get; set; }
        public string QuestState { get; set; }
    }
}