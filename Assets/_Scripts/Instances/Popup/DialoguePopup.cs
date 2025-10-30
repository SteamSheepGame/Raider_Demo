using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.UIElements;

namespace Demo.Core
{
    public class DialoguePopup: BasePopup
    {
        [TitleGroup("UI References")] 
        [SerializeField, Required] private TextMeshProUGUI title;
        [SerializeField, Required] private UnityEngine.UI.Button FirstChoiceButton;
        [SerializeField, Required] public UnityEngine.UI.Button SecondChoiceButton;
        [SerializeField, Required] private TextMeshProUGUI narration;
        [SerializeField, Required] private RectTransform scrollAreaRect;
        [SerializeField, Required] private GameObject dialoguePrefab;
        

        private Dictionary<string, DialogueBinding> Dialogues = new Dictionary<string, DialogueBinding>();

        private DialogueBinding currDialogue;
        public override void Bind(IEntity entity)
        {
            base.Bind(entity);
            DialoguePopupEntity dialoguePopupEntity = entity as DialoguePopupEntity;
            
            if (dialoguePopupEntity == null)
            {
                Debug.LogError("LocationPopup Bind error");
            }

            foreach (DialogueBinding dialogue in dialoguePopupEntity.Dialogues)
            {
                Dialogues.Add(dialogue.Id, dialogue);
            }

            FirstChoiceButton.onClick.AddListener(()=>Reply(0));
            SecondChoiceButton.onClick.AddListener(()=>Reply(1));
            
            StartDialogue(dialoguePopupEntity.Dialogues[0]);
        }

        private void Reply(int index)
        {
            if (currDialogue != null && currDialogue.Replies.Count > 1)
            {
                Replies reply = currDialogue.Replies[index];
                GameObject dialogueInstance = Instantiate(dialoguePrefab, scrollAreaRect);
                TextMeshProUGUI speakerName = dialogueInstance.transform.Find("SpeakerName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI dialogueText = dialogueInstance.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
                // speakerName.text = currDialogue.Replies[0].Speaker;
                dialogueText.text = reply.Text;
                
                StartDialogue(Dialogues[reply.NextDialogueId]);
            }
            else
            {
                FirstChoiceButton.gameObject.SetActive(false);
                SecondChoiceButton.gameObject.SetActive(false);
                // Debug.LogError("No reply found.");
            }
        }

        private void StartDialogue(DialogueBinding currentDialogue)
        {
            if (currentDialogue == null)
            {
                Debug.LogError("LocationPopup StartDialogue error");
                return;
            }
            currDialogue = currentDialogue;
            
            FirstChoiceButton.gameObject.SetActive(false);
            SecondChoiceButton.gameObject.SetActive(false);
            // FirstChoiceButton.enabled = false;
            // SecondChoiceButton.enabled = false;
            
            narration.text = currentDialogue.Narration;
            
            if (currentDialogue.Replies.Count > 1)
            {
                FirstChoiceButton.GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.Replies[0]?.ButtonText;
                SecondChoiceButton.GetComponentInChildren<TextMeshProUGUI>().text = currentDialogue.Replies[1]?.ButtonText;
            }
            
            GameObject dialogueInstance = Instantiate(dialoguePrefab, scrollAreaRect);
            TextMeshProUGUI speakerName = dialogueInstance.transform.Find("SpeakerName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI dialogueText = dialogueInstance.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
            speakerName.text = currentDialogue.Speaker;
            dialogueText.text = currentDialogue.Text;
            
            StartReply();
        }

        private void StartReply()
        {
            FirstChoiceButton.gameObject.SetActive(true);
            SecondChoiceButton.gameObject.SetActive(true);
        }
        
        
    }
}