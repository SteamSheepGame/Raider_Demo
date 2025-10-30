using Sirenix.OdinInspector;
using TMPro;
using UnityEditor.Graphs;
using UnityEngine;
namespace Demo.Core
{
    public class LocationPopup: BasePopup
    {
        [SerializeField, Required] TextMeshProUGUI locationName;
        [SerializeField, Required] TextMeshProUGUI locationDescription;
        [SerializeField, Required] private RectTransform SlotRect;
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
                // slot.Rect.anchoredPosition = SlotRect.anchoredPosition;
                slot.Rect.parent = SlotRect;
                slot.Rect.anchoredPosition = Vector2.zero;
            }

        }
    }
}