using UnityEngine; 
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
        Object Parent { get; set; }
        ICard FilledCard { get; }

        //void Place(ICard card);
        public bool TryAccept(ICard card);
        void Highlight(bool on);
        void Clear();
    }
}