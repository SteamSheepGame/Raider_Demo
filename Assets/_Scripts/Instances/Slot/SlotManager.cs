using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Demo.Core
{
    public class SlotManager : Singleton<SlotManager>
    {
        private readonly List<ISlot> _RegisteredSlot = new();
        public void Register(ISlot slot)
        {
            if (slot == null) return;
            _RegisteredSlot.Add(slot);
        }

        public void Unregister(ISlot slot)
        {
            if (slot == null) return;  
            _RegisteredSlot.Remove(slot);
        }

        public ISlot GetSlot(int index)
        {
            return _RegisteredSlot[index];
        }

        public ISlot GetNearestOverlapping(RectTransform cardRect)
        {
            var center = (Vector2)cardRect.position;
            ISlot Best = null;
            float BestDist = float.MaxValue;

            foreach (var slot in _RegisteredSlot)
            {
                if (slot == null) continue;
                var Rt = slot.Rect;
                if (Rt == null || !Rt.gameObject.activeInHierarchy) continue;

                if (CheckOverlap(cardRect, Rt)) // only overlapping candidates
                {
                    float Dist = Vector2.SqrMagnitude((Vector2)Rt.position - center);
                    if (Dist < BestDist)
                    {
                        BestDist = Dist;
                        Best = slot;
                    }
                }
            }
            
            return Best;
        }

        private bool CheckOverlap(RectTransform cardRect, RectTransform overlappingRect)
        {
            Vector2 cardWorldPosition = cardRect.TransformPoint(cardRect.rect.center);
            Vector2 slotWorldPosition = overlappingRect.TransformPoint(overlappingRect.rect.center);
            // Vector2 cardPivotWorld = cardRect.anchoredPosition;
            // Vector2 slotPivotWorld = overlappingRect.anchoredPosition;

            float thresholdDistance = Mathf.Max(cardRect.rect.height, cardRect.rect.width);
            float distanceSqr = (cardWorldPosition - slotWorldPosition).sqrMagnitude;
            
            return distanceSqr <= thresholdDistance * thresholdDistance;
        }
    }
}