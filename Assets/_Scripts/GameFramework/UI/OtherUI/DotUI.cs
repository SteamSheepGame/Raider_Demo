using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
namespace Demo.Core
{
    public class DotUI: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private RectTransform dotRect;
        [SerializeField] private string locationPopupId;
        [SerializeField] private float hoverMultiplier = 1.25f;
        
        private Vector3 originalScale;
        private void Awake()
        {
            originalScale = dotRect.localScale;
        }

        public void Init(string popupId)
        {
            locationPopupId = popupId;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            dotRect.localScale = originalScale * hoverMultiplier;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            dotRect.localScale = originalScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Pull the entity from your database
            IEntityStoreService store = ServiceProvider.Instance.GetService<IEntityStoreService>();
            LocationPopupEntity entity = store.GetEntity(locationPopupId) as LocationPopupEntity;

            if (entity == null)
            {
                Debug.LogError($"[DotUI] Cannot find LocationPopupEntity: {locationPopupId}");
                return;
            }
            
            // 生存Popup
            UIManager.Instance.SpawnPopup(locationPopupId);
        }
    }
}