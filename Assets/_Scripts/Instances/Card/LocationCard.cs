using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.EventSystems;
namespace Demo.Core
{
    public class LocationCard: BaseCard<LocationCard>
    {
        private void OnEnable()
        {
            IsDraggable = false;
        }

        public override void Bind(IEntity entity)
        {
            // 链接Entity
            Entity = entity;
            LocationEntity Location = entity as LocationEntity;
            
            // 初始化卡片UI
            Label = Location.Label;
            Background = TryLoadSpriteFromResources(Location.Image);

            foreach (var Action in Location.AvailableActions)
            {
                if (Action.Trigger.Equals("OnClick", StringComparison.OrdinalIgnoreCase))
                {
                    OnClicked += (_) => ServiceProvider.Instance.GetService<IActionService>().ExecuteAction(Action);
                }
            }
        }
    }
}