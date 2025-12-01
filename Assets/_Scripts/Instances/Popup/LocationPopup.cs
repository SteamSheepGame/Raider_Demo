using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Demo.Core
{
    public class LocationPopup: BasePopup
    {
        [SerializeField, Required] TextMeshProUGUI locationName;
        [SerializeField, Required] TextMeshProUGUI locationDescription;
        [SerializeField, Required] private RectTransform SlotRect;
        [SerializeField, Required] private Button confirmButton;
        [SerializeField] private Image progressBar;
        
        private ISlot currSlot;
        private bool workCompleted = false;
        private event Action onConfirmed;
        private void Awake()
        {
            confirmButton.onClick.AddListener(OnConfirmClicked);
        }
        
        public override void Bind(IEntity entity)
        {
            base.Bind(entity);
            LocationPopupEntity locationPopupEntity = entity as LocationPopupEntity;
            
            if (locationPopupEntity == null)
            {
                Debug.LogError("LocationPopup Bind error");
            }
            
            locationName.text = locationPopupEntity.Title;
            locationDescription.text = locationPopupEntity.Description;

            foreach (var slot in locationPopupEntity.Slots)
            {
                // Get Entity
                IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
                CharacterSlotEntity characterSlot = storeService.GetEntity(slot) as CharacterSlotEntity;
                if (entity == null)
                {
                    Debug.LogError("[UIManager] Entity not found.");
                }
                
                SpawnSlot(characterSlot);
            }
            
            
            // Bind action for onConfirm button
            foreach (var action in locationPopupEntity.AvailableActions)
            {
                if (action.Trigger.Equals("onConfirm", StringComparison.OrdinalIgnoreCase))
                {
                    onConfirmed += () => ServiceProvider.Instance.GetService<IActionService>().ExecuteAction(action);
                }
            }
            
        }

        private void Update()
        {
            if (currentTask != null)
            {
                var c = currentTask.WorkTimer;
                float progress = 1.0f;
                if (!workCompleted)
                {
                    progress = TimeSystem.Instance.GetProgress(c);
                    progressBar.fillAmount = progress;
                } 

                Debug.Log("ProgressBar: " + progress);
                
                // allow confirming only when finished
                confirmButton.interactable = (progress >= 1f);
            }
        }

        public override void StartWork()
        {
            base.StartWork();
            currSlot.LockCard();
        }

        void OnConfirmClicked()
        {
            workCompleted = false;
            
            currSlot.UnlockCard();
            UIManager.Instance.ClosePopup(PopupId);
            
            onConfirmed?.Invoke();
        }
        
        public override void OnWorkFinished()
        {
            base.OnWorkFinished();
            workCompleted = true;
            progressBar.fillAmount = 1;
            confirmButton.interactable = true;
        }

        /// <summary>
        /// LocationPopup生成Slot的helper方法
        /// </summary>
        /// <param name="slotEntity"></param>
        private void SpawnSlot(CharacterSlotEntity slotEntity)
        {
            // Spawn Slot
            IFactoryService factoryService = ServiceProvider.Instance.GetService<IFactoryService>();
            ISlot slot = factoryService.Create(slotEntity) as ISlot;
            // Position Slot
            if (slot != null)
            {
                currSlot = slot;
                // slot.Rect.anchoredPosition = SlotRect.anchoredPosition;
                slot.Rect.parent = SlotRect;
                slot.Rect.anchoredPosition = Vector2.zero;
            }

        }
        
        public override void OnPopupClosed()
        {
            if(currSlot != null)
            {
                currSlot.ReturnCard();
            }
        }
    }
}