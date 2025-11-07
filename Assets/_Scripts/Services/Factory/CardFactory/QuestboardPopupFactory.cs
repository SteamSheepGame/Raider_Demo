using UnityEngine;
namespace Demo.Core
{
    public class QuestboardPopupFactory: Factory<IPopup>
    {
        public QuestboardPopupFactory(GameObject prefab) : base(prefab)
        {
        }

        protected override IPopup CreateInstance(IEntity entity = null)
        {
            if (entity == null)
            {
                Debug.Log("Creating new instance of LocationPopupFactory, but entity is null");
            }
            
            
            QuestboardPopupEntity questPopupEntity = entity as QuestboardPopupEntity;
            if (questPopupEntity == null)
            {
                Debug.Log("PopupFactory::CreateInstance: entity is not PopupEntity");
                return null;
            }
            
            Transform parentCanvas = UIManager.Instance.GUIView.transform;
            // Set name for UIManager/GUI
            var popupObject = Object.Instantiate(Prefab, parentCanvas);
            popupObject.name = questPopupEntity.Id;

            IPopup PopupComp = popupObject.GetComponent<QuestboardPopup>();
            PopupComp.Bind(questPopupEntity);
            return PopupComp;
        }
    }
}