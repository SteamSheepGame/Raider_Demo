using System;          
using UnityEngine; 
namespace Demo.Core
{
    public interface ICard : IInstance
    {
        IEntity Entity { get; }
        public void Bind(IEntity entity);
        
        // Visuals
        string Label { get; set; }
        Sprite Background { get; set; }
        RectTransform Rect { get; }
        
        ISlot OccupiedSlot { get; }
        /*IDeck<CharacterCard> ParentDeck { get; }*/
        
        // State
        bool IsSelected { get; set; } 
        bool IsDraggable { get; set; }
        bool IsFaceUp { get; set; }
        
        // UX actions
        void Expand();                  
        void Highlight(bool on);        
        void Select(bool on);
        
        void PlaceCard(ISlot slot);
        // void MoveTo(Vector3 worldPos, float duration = 0.15f); 
        void SnapTo(ISlot slot);

        void SetParentDeck(IDeck deck);
        
        // Signals
        event Action<ICard> Clicked;
        event Action<ICard> BeginDrag;
        event Action<ICard> EndDrag;
        event Action<ICard, ISlot> DroppedOnSlot;
    }
}