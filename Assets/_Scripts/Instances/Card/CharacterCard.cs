using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Demo.Core
{
    public class CharacterCard: BaseCard<CharacterCard>
    {
        public override void Bind(IEntity entity)
        {
            // 链接Entity
            Entity = entity;
            CharacterEntity Character = entity as CharacterEntity;
            
            // 初始化卡片UI
            Label = Character.Label;
            Background = TryLoadSpriteFromResources(Character.Image);
            
            foreach (var Action in Character.AvailableActions)
            {
                if (Action.Trigger.Equals("OnClick", StringComparison.OrdinalIgnoreCase))
                {
                    OnClicked += (_) => ServiceProvider.Instance.GetService<IActionService>().ExecuteAction(Action);
                }
            }
        }

        public override void AddToDeck()
        {
            if (ParentDeck != null)
            {
                ParentDeck.TryAdd(this);
            }
            else
            {
                //TODO
            }
        }
    }
}