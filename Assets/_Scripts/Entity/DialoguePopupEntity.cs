using System;
using System.Collections.Generic;
namespace Demo.Core
{
    [DataImportable("DialoguePopup")]
    public class DialoguePopupEntity: IEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public List<ActionBinding> AvailableActions { get; set; } = new();
        public List<DialogueBinding> Dialogues{ get; set; }
    }

    [Serializable]
    public class DialogueBinding
    {
        public string Id { get; set; }
        public string Speaker { get; set; }
        public string Narration { get; set; }
        public string Text { get; set; }
        public List<Replies> Replies { get; set; }
    }

    [Serializable]
    public class Replies
    {
        public string Id { get; set; }
        public string ButtonText { get; set; }
        public string Text { get; set; }
        public string NextDialogueId { get; set; }
    }
}