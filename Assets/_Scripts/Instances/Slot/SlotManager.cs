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
            Vector3[] aCorners = new Vector3[4];
            Vector3[] bCorners = new Vector3[4];
            cardRect.GetWorldCorners(aCorners);
            overlappingRect.GetWorldCorners(bCorners);

            Rect rectA = new Rect(aCorners[0].x, aCorners[0].y,
                aCorners[2].x - aCorners[0].x, aCorners[2].y - aCorners[0].y);
            Rect rectB = new Rect(bCorners[0].x, bCorners[0].y,
                bCorners[2].x - bCorners[0].x, bCorners[2].y - bCorners[0].y);

            return rectA.Overlaps(rectB);
        }
    }
}