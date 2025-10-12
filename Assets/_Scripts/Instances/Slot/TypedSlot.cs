using UnityEngine; 
using Sirenix.OdinInspector;
namespace Demo.Core
{
    public class TypedSlot<TCard>: BaseSlot where TCard : ICard
    {
        /// <summary>
        /// 尝试填充卡片
        /// </summary>
        /// <param name="card"></param>
        public override bool TryAccept(ICard card)
        {
            // 如果可填充，填充slot
            if (card is TCard && !IsFilled)
            {
                Place(card);
                return true;
            }
            return false;
        }
    }
}