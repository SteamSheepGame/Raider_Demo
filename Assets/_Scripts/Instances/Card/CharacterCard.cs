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
        }
    }
}