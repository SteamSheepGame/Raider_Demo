using System.Collections.Generic;
using UnityEngine; 
namespace Demo.Core
{
    public interface IDeck
    {
        RectTransform Rect { get; }
        int GetInsertIndexFromLocalPosition(Vector2 localPos);
    }
}