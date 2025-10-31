using UnityEngine; 
using System;          

namespace Demo.Core
{
    public interface ISlot : IInstance
    {
        // 状态
        bool IsFilled { get; set; }
        bool IsSelected { get; set; }
        // UI
        Sprite Background { get; set; }
        RectTransform Rect { get; }
        // 物品Reference
        UnityEngine.Object Parent { get; set; }
        ICard FilledCard { get; }

        void Bind(IEntity entity);

        //void Place(ICard card);
        public bool TryAccept(ICard card);
        void Highlight(bool on);
        void Clear();

        void ReturnCard();
        
        // Events
        event Action<ISlot> HoveredEnd;
        event Action<ISlot> HoveredStart;
        event Action<ISlot, ICard> Filled;      
        event Action<ISlot, ICard> Cleared;   
    }
}