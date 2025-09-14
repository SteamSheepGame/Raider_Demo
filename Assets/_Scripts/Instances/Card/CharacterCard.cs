using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using TMPro;

namespace Demo.Core
{
    public class CharacterCard: SerializedMonoBehaviour, ICard
    {
		// Marked for change
        public bool IsDraggable { get; set; }
        public bool IsSelected { get; set; }
        public IEntity Entity { get; private set;}
        
        /// <summary>
        /// 与entity类链接
        /// </summary>
        /// <param name="entity"></param>
        public void Bind(IEntity entity)
        {
            Entity = entity;
            
            // Testing
            DisplayTest();
        }
        
        /// <summary>
        /// Testing (Will be removed later)
        /// </summary>
        private void DisplayTest()
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