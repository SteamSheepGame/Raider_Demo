using UnityEngine;
namespace Demo.Core
{
    public class PopupFactory: Factory<IPopup>
    {
        public PopupFactory(GameObject prefab) : base(prefab)
        {
        }

        protected override IPopup CreateInstance(IEntity entity = null)
        {
            if (entity == null)
            {
                Debug.Log("Creating new instance of PopupFactory, but entity is null");
            }
            
            
            PopupEntity popupEntity = entity as PopupEntity;
            if (popupEntity == null)
            {
                Debug.Log("PopupFactory::CreateInstance: entity is not PopupEntity");
                return null;
            }

            switch (popupEntity.Type)
            {
                case "LocationPopup":
                {
                    // LocationPopup popup = new LocationPopup();
                   
                    Transform parentCanvas = UIManager.Instance.HUDView.transform;
                    // Set name for UIManager/GUI
                    var popupObject = Object.Instantiate(Prefab, parentCanvas);
                    popupObject.name = popupEntity.Id;

                    IPopup PopupComp = popupObject.GetComponent<LocationPopup>();
                    PopupComp.Bind(popupEntity);
                    return PopupComp;
                }
            }

            return null;
        }
    }
}