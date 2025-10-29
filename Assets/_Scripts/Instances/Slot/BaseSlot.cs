using System;
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
        [SerializeField] private Image _backgroundImage;
        
        // Event
        public event Action<ISlot> HoveredStart;
        public event Action<ISlot> HoveredEnd;
        public event Action<ISlot, ICard> Filled;      
        public event Action<ISlot, ICard> Cleared;   
        
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

        public virtual void Bind(IEntity entity)
        {
            
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
        public  UnityEngine.Object Parent { get; set; }
        public ICard FilledCard { get; private set; }
        
        // 注册到Manager
        void OnEnable()  => SlotManager.Instance?.Register(this);
        void OnDisable() => SlotManager.Instance?.Unregister(this);

        public virtual bool TryAccept(ICard card)
        {
            if (IsFilled) return false;
            Place(card);
            return true;
        }
        protected void Place(ICard card)
        {
            //card.MoveTo(_rect.anchoredPosition);
            FilledCard = card;
            IsFilled = true;    
            Filled?.Invoke(this, card);
            
            card.PlaceCard(this);
        }
        
        public void Highlight(bool on)
        {
            if (on)
            {
                HoveredStart?.Invoke(this);
            }
            else
            {
                HoveredEnd?.Invoke(this);
            }
            IsSelected = on;
        }

        public void Clear()
        {
            IsFilled = false;
            FilledCard = null;
            Cleared?.Invoke(this, FilledCard);
        }
    }
}