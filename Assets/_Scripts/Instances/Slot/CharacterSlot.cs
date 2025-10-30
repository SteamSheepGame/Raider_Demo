using System;
using UnityEngine;
namespace Demo.Core
{
    public class CharacterSlot: TypedSlot<CharacterCard>
    {
        public override void Bind(IEntity entity)
        {
            base.Bind(entity);
            
            CharacterSlotEntity Slot = entity as CharacterSlotEntity;
            foreach (var Action in Slot.AvailableActions)
            {
                if (Action.Trigger.Equals("OnFill", StringComparison.OrdinalIgnoreCase))
                {
                    Filled += (_,_) => ServiceProvider.Instance.GetService<IActionService>().ExecuteAction(Action);
                }
            }
        }
    }
}