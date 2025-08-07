using UnityEngine;
using TMPro;

namespace Demo.Core
{
    public class CharacterCardView: MonoBehaviour, ICardView
    {
		// Marked for change
        public bool IsDraggable { get; set; }
        public bool IsSelected { get; set; }
        public IEntity Entity { get; private set;}
        
        /// <summary>
        /// 与entity类链接
        /// </summary>
        /// <param name="entity"></param>
        public void bind(IEntity entity)
        {
            Entity = entity;
            
            // Testing
            displayTest();
        }
        
        /// <summary>
        /// Testing (Will be removed later)
        /// </summary>
        public void displayTest()
        {
            // Testing
            GameObject textObj = new GameObject("NameText");
            textObj.transform.SetParent(this.transform);

            var text = textObj.AddComponent<TextMeshProUGUI>();
            text.text = (Entity as CharacterEntity)?.Label;
            text.fontSize = 24;
            text.alignment = TextAlignmentOptions.Center;

            // Position the text (optional)
            var rectTransform = text.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
        }
        
        /// <summary>
        /// 扩展到细节面板
        /// </summary>
        public void Expand()
        {
            
        }
    }
}