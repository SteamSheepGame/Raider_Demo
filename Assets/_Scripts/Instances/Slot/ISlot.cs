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
        
        // 物品Reference
        Object Parent { get; set; }
        ICard FilledCard { get; }

        void Place(ICard card);
        void Highlight(bool on);    
    }
}