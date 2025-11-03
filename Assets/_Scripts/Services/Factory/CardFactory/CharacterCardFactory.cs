using UnityEngine;
namespace Demo.Core
{
    /// <summary>
    /// 后续考虑加一层CardFactory
    /// </summary>
    public class CharacterCardFactory : Factory<ICard>
    {
        public CharacterCardFactory(GameObject prefab) : base(prefab)
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
            Transform parentCanvas = UIManager.Instance.GUIView.transform;
            var cardObject = Object.Instantiate(Prefab, parentCanvas);
            
            // 初始化ICard
            ICard card = cardObject.GetComponent<CharacterCard>();
            card.Bind(entity);
            return card;
        }
    }
}