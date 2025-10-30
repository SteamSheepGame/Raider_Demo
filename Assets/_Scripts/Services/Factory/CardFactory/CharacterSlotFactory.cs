using UnityEditor.Graphs;
using UnityEngine;
namespace Demo.Core
{
    public class CharacterSlotFactory: Factory<ISlot>
    {
        public CharacterSlotFactory(GameObject prefab) : base(prefab)
        {
        }

        protected override ISlot CreateInstance(IEntity entity = null)
        {
            if(entity == null) Debug.Log("Creating instance of Slot without entity!");
            
            Transform parentCanvas = UIManager.Instance.HUDView.transform;
            var cardObject = Object.Instantiate(Prefab, parentCanvas);
            
            // 初始化ISlot
            ISlot slot = cardObject.GetComponent<CharacterSlot>();
            slot.Bind(entity);
            return slot;
        }
    }
}