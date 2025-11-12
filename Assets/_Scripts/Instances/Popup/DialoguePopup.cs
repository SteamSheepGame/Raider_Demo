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

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="index"></param>
        private void Reply(int index)
        {
            if (currDialogue != null && currDialogue.Replies.Count > 0)
            {
                // 生成回复
                Replies reply = currDialogue.Replies[index];
                GameObject dialogueInstance = Instantiate(dialoguePrefab, scrollAreaRect);
                TextMeshProUGUI speakerName = dialogueInstance.transform.Find("SpeakerName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI dialogueText = dialogueInstance.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
                // speakerName.text = currDialogue.Replies[0].Speaker;
                dialogueText.text = reply.Text;
                // 开始回复
                StartDialogue(Dialogues[reply.NextDialogueId]);
            }
            else
            {
                // 隐藏按钮
                FirstChoiceButton.gameObject.SetActive(false);
                SecondChoiceButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 开始对话
        /// </summary>
        /// <param name="currentDialogue"></param>
        private void StartDialogue(DialogueBinding currentDialogue)
        {
            
            if (currentDialogue == null)
            {
                Debug.LogError("LocationPopup StartDialogue error");
                return;
            }
            // 设置本地变量
            currDialogue = currentDialogue;
            // 隐藏回复按钮
            FirstChoiceButton.gameObject.SetActive(false);
            SecondChoiceButton.gameObject.SetActive(false);
            // 旁白
            narration.text = currentDialogue.Narration;
            // 对话者
            GameObject dialogueInstance = Instantiate(dialoguePrefab, scrollAreaRect);
            TextMeshProUGUI speakerName = dialogueInstance.transform.Find("SpeakerName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI dialogueText = dialogueInstance.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
            speakerName.text = currentDialogue.Speaker;
            dialogueText.text = currentDialogue.Text;
            // 回复
            StartReply();
        }

        private void StartReply()
        {
            // 如有reply，显示button
            if (currDialogue.Replies.Count > 0)
            {
                FirstChoiceButton.GetComponentInChildren<TextMeshProUGUI>().text = currDialogue.Replies[0]?.ButtonText;
                FirstChoiceButton.gameObject.SetActive(true);
            }

            if (currDialogue.Replies.Count > 1)
            {
                SecondChoiceButton.GetComponentInChildren<TextMeshProUGUI>().text = currDialogue.Replies[1]?.ButtonText;
                SecondChoiceButton.gameObject.SetActive(true);
            }
        }
        
        
    }
}