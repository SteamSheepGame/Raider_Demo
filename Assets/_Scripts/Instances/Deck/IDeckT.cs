using System.Collections.Generic;
using UnityEngine; 
namespace Demo.Core
{
    public interface IDeck<TCard> : IDeck where TCard : ICard
    {
        RectTransform Rect { get; }
        void InitDeck(List<TCard> Cards);
        bool TryAdd(TCard card);
        bool TryInsert(TCard card, int index);
        bool TryRemove(TCard card);
        
        void OnDeckHoverStart(int index);
        void OnDeckHoverEnd(int index);
    }
}