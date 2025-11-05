using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

namespace Demo.Core
{
    public class CharacterPopup: BasePopup
    {
        [SerializeField, Required] TextMeshProUGUI CharacterName;
        [SerializeField, Required] TextMeshProUGUI CharacterDescription;
        [SerializeField, Required] private RectTransform SlotRect;

        public override void Bind(IEntity entity)
        {
            base.Bind(entity);
            CharacterPopupEntity cEntity = entity as CharacterPopupEntity;
            
            CharacterName.text = cEntity.Name;
            CharacterDescription.text = cEntity.Description;
        }
    }
}