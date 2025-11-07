using UnityEngine;

namespace Demo.Core
{
    public class CharacterPopupFactory: Factory<IPopup>
    {
        public CharacterPopupFactory(GameObject prefab) : base(prefab)
        {
        }

        protected override IPopup CreateInstance(IEntity entity = null)
        {
            if (entity == null)
            {
                Debug.Log("Creating new instance of CharacterPopupFactory, but entity is null");
            }
            
            CharacterPopupEntity characterPopupEntity = entity as CharacterPopupEntity;
            if (characterPopupEntity == null)
            {
                Debug.Log("PopupFactory::CreateInstance: entity is not PopupEntity");
                return null;
            }
            
            Transform parentCanvas = UIManager.Instance.GUIView.transform;
            // Set name for UIManager/GUI
            var popupObject = Object.Instantiate(Prefab, parentCanvas);
            popupObject.name = characterPopupEntity.Id;

            IPopup PopupComp = popupObject.GetComponent<CharacterPopup>();
            PopupComp.Bind(characterPopupEntity);
            return PopupComp;
        }
    }
}