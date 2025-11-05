using UnityEngine;
namespace Demo.Core
{
    public class DialoguePopupFactory: Factory<IPopup>
    {
        public DialoguePopupFactory(GameObject prefab) : base(prefab)
        {
        }

        protected override IPopup CreateInstance(IEntity entity = null)
        {
            if (entity == null)
            {
                Debug.Log("Creating new instance of DialoguePopupFactory, but entity is null");
            }
            
            
            DialoguePopupEntity dialoguePopupEntity = entity as DialoguePopupEntity;
            if (dialoguePopupEntity == null)
            {
                Debug.Log("PopupFactory::CreateInstance: entity is not PopupEntity");
                return null;
            }
            
            Transform parentCanvas = UIManager.Instance.GUIView.transform;
            // Set name for UIManager/GUI
            var popupObject = Object.Instantiate(Prefab, parentCanvas);
            popupObject.name = dialoguePopupEntity.Id;

            IPopup PopupComp = popupObject.GetComponent<DialoguePopup>();
            PopupComp.Bind(dialoguePopupEntity);
            return PopupComp;
        }
    }
}