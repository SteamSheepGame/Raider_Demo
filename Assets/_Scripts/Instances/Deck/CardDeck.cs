using System;
using UnityEngine; 
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEditor.Graphs;
using Object = UnityEngine.Object;

namespace Demo.Core
{
    public class CardDeck<TCard>: SerializedMonoBehaviour, IDeck<TCard> where TCard : ICard
    {
        [TitleGroup("UI References")]
        [SerializeField, Required] protected RectTransform _rect;
        [SerializeField, Required] protected int _maxCount = 10;
        [SerializeField, Required] protected float xOffset = 200;
        
        protected int currentCount = 0;
        
        protected List<TCard> _cards = new List<TCard>();

        private bool _clearSignalSuppressed = false;
        private bool _fillSignalSuppressed = false;
        private bool _hoverSignalSuppressed = false;

        private int lastHoveredSlotIndex = -1;
        
        public RectTransform Rect
        {
            get => _rect;
            set => _rect = value;
        }
        
        public void InitDeck(List<TCard> Cards)
        {
            if (Cards.Count > _maxCount)
            {
                Debug.Log("Init Deck exceed max count");
            }
            else
            {
                foreach (TCard card in Cards)
                {
                    TryAdd(card);
                }
            }
        }

        private void UpdatePosition()
        {
            // 算出card数量
            int visibleCount = _cards.Count;
            bool hasHoverHole = lastHoveredSlotIndex >= 0 && lastHoveredSlotIndex <= visibleCount;
            if (hasHoverHole) visibleCount += 1;

            // 算出最左位置
            float totalWidth = (visibleCount - 1) * xOffset;
            float xStart = -0.5f * totalWidth;

            // 排布每个card
            for (int i = 0, visualIndex = 0; i < _cards.Count; i++, visualIndex++)
            {
                // If we're at the hover index, skip one space
                if (hasHoverHole && visualIndex == lastHoveredSlotIndex)
                {
                    visualIndex++;
                }

                Vector2 pos = new Vector2(xStart + visualIndex * xOffset, 0f);
                _cards[i].Rect.anchoredPosition = pos;
            }
        }
        
        public bool TryAdd(TCard card)
        {
            if (currentCount >= _maxCount) return false;
            
            _cards.Add(card);
            card.Rect.SetParent(Rect, false);
            currentCount++;
            
            //
            card.SetParentDeck(this);

            UpdatePosition();
            return true;
        }

        public bool TryInsert(TCard card, int index)
        {
            if (index < 0) index = 0;
            if (index > _cards.Count) index = _cards.Count;

            if (currentCount >= _maxCount)
                return false;
            
            _cards.Insert(index, card);
            card.Rect.SetParent(Rect, false);
            currentCount++;
            
            card.SetParentDeck(this);

            UpdatePosition();
            return true;
        }

        public bool TryRemove(TCard card)
        {
            if (_cards.Remove(card))
            {
                UpdatePosition();
                currentCount--;
                return true;
            }
            return false;
        }

        public void OnDeckHoverStart(int index)
        {
            lastHoveredSlotIndex = Mathf.Clamp(index, 0, _cards.Count);
            UpdatePosition();
        }
        
        public void OnDeckHoverEnd(int index)
        {
            lastHoveredSlotIndex = -1;
            UpdatePosition();
        }
        
        public int GetInsertIndexFromLocalPosition(Vector2 localMousePos)
        {
            if (_cards.Count == 0) return 0;

            float startX = -0.5f * ((_cards.Count) * xOffset);
            float relativeX = localMousePos.x - startX;
            return Mathf.Clamp(Mathf.RoundToInt(relativeX / xOffset), 0, _cards.Count);
        }
    }
}