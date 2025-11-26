using UnityEngine;
using System.Collections.Generic;
namespace Demo.Core
{
    public class LocationPopupFactory: Factory<IPopup>
    {
        public LocationPopupFactory(GameObject prefab) : base(prefab)
        {
        }

        protected override IPopup CreateInstance(IEntity entity = null)
        {
            if (entity == null)
            {
                Debug.Log("Creating new instance of LocationPopupFactory, but entity is null");
            }
            
            
            LocationPopupEntity locationPopupEntity = entity as LocationPopupEntity;
            if (locationPopupEntity == null)
            {
                Debug.Log("PopupFactory::CreateInstance: entity is not PopupEntity");
                return null;
            }
            
            Transform parentCanvas = UIManager.Instance.GUIView.transform;
            // Set name for UIManager/GUI
            var popupObject = Object.Instantiate(Prefab, parentCanvas);
            popupObject.name = locationPopupEntity.Id;

            IPopup PopupComp = popupObject.GetComponent<LocationPopup>();
            PopupComp.Bind(locationPopupEntity);
            return PopupComp;
           

            return null;
        }
    }
}