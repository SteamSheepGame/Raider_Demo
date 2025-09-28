using UnityEngine; 
using Sirenix.OdinInspector;

namespace Demo.Core
{
    public abstract class BaseSlot : SerializedMonoBehaviour, ISlot
    {
        [TitleGroup("UI References")] 
        [SerializeField, Required] private RectTransform _rect;
        
        // 状态
        public bool IsFilled { get; set; }
        public bool IsSelected { get; set; }
        
        // UI
        public Sprite Background { get; set; }
        
        // 物品Reference
        public Object Parent { get; set; }
        public ICard FilledCard { get; private set; }
        
        public abstract bool TryAccept(ICard card);
        public void Place(ICard card)
        {
            card.MoveTo(_rect.position);
            FilledCard = card;
            IsFilled = true;
        }
        
        public void Highlight(bool on)
        {
            IsSelected = on;
        }
    }
}