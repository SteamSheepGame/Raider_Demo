using UnityEngine; 
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEditor.Graphs;

namespace Demo.Core
{
    public abstract class BaseSlot : SerializedMonoBehaviour, ISlot
    {
        [TitleGroup("UI References")] 
        [SerializeField, Required] private RectTransform _rect;
        [SerializeField, Required] private Image _backgroundImage;
        
        // 状态
        public bool IsFilled { get; set; }
        public bool IsSelected { get; set; }
        
        public bool IsActive { get; set; }

        public RectTransform Rect
        {
            get => _rect != null? _rect : null;
            set
            {
                if (_rect != null) _rect = value;
            }
        }
        
        // UI
        public Sprite Background
        {
            get => _backgroundImage != null ? _backgroundImage.sprite : null;
            set
            {
                if (_backgroundImage != null) _backgroundImage.sprite = value;
            }
        }
        
        // 物品Reference
        public Object Parent { get; set; }
        public ICard FilledCard { get; private set; }
        
        // 注册到Manager
        void OnEnable()  => SlotManager.Instance?.Register(this);
        void OnDisable() => SlotManager.Instance?.Unregister(this);
        
        public abstract bool TryAccept(ICard card);
        protected void Place(ICard card)
        {
            card.MoveTo(_rect.anchoredPosition);
            FilledCard = card;
            IsFilled = true;    
        }
        
        public void Highlight(bool on)
        {
            IsSelected = on;
        }

        public void Clear()
        {
            IsFilled = false;
            FilledCard = null;
        }
    }
}