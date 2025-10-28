using UnityEngine;
namespace Demo.Core
{
    public class LocationCardFactory: Factory<ICard>
    {
        public LocationCardFactory(GameObject prefab) : base(prefab)
        {
        }
        
        /// <summary>
        /// 重写虚方法，创建卡片，建立Card与Entity的链接
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override ICard CreateInstance(IEntity entity = null)
        {
            if(entity == null) Debug.Log("Creating instance of CharacterCard without entity!");
            // Get UI Canvas
            Transform parentCanvas = UIManager.Instance.HUDView.transform;
            var cardObject = Object.Instantiate(Prefab, parentCanvas);
            
            // 初始化ICard
            ICard card = cardObject.GetComponent<LocationCard>();
            card.Bind(entity);
            return card;
        }
    }
}