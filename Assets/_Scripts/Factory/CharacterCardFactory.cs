using UnityEngine;
namespace Demo.Core
{
    public class CharacterCardFactory : ICardFactory
    {
        // 一个基本的card prefab
        private GameObject _cardPrefab;
        
        /// <summary>
        /// 设置单例Factory的prefab
        /// </summary>
        /// <param name="cardPrefab"></param>
        public CharacterCardFactory(GameObject cardPrefab)
        {
            _cardPrefab = cardPrefab;
        }
        
        /// <summary>
        /// 泛用创建卡片，建立CardView与Entity的链接
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ICardView CreateCard(IEntity entity)
        {
            var Card = GameObject.Instantiate(_cardPrefab);
            ICardView CardView = Card.AddComponent<CharacterCardView>();
            CardView.bind(entity);
            return CardView;
        }
    }
}