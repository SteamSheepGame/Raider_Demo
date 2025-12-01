using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Demo.Core
{
    public class CharacterPopup: BasePopup
    {
        [SerializeField, Required] TextMeshProUGUI CharacterName;
        [SerializeField, Required] TextMeshProUGUI CharacterDescription;
        [SerializeField, Required] private RectTransform SlotRect;
        [SerializeField, Required] Image characterImage;

        public override void Bind(IEntity entity)
        {
            base.Bind(entity);
            CharacterPopupEntity cEntity = entity as CharacterPopupEntity;
            
            CharacterName.text = cEntity.Name;
            CharacterDescription.text = cEntity.Description;
            characterImage.sprite = TryLoadSpriteFromResources(cEntity.Image);
        }
        
        protected static Sprite TryLoadSpriteFromResources(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            var sprite = Resources.Load<Sprite>(path);
            if (!sprite)
                Debug.LogWarning($"[CharacterCardDetailView] Sprite not found at Resources path: {path}");
            return sprite;
        }
    }
}