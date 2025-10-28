using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
namespace Demo.Core
{
    public class LocationPopup: BasePopup
    {
        [SerializeField, Required] TextMeshProUGUI locationName;
        [SerializeField, Required] TextMeshProUGUI locationDescription;
        public override void Bind(IEntity entity)
        {
            base.Bind(entity);
            PopupEntity popupEntity = entity as PopupEntity;
            
            if (popupEntity == null)
            {
                Debug.LogError("LocationPopup Bind error");
            }
            
            locationName.text = popupEntity.Title;
            locationDescription.text = popupEntity.Description;
        }
    }
}