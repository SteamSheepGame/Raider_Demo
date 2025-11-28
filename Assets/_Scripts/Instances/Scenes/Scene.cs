using System.Collections.Generic;
using Sirenix.Utilities;

namespace Demo.Core
{
    /// Used for loading Locations/Dialogues/..
    public class Scene: IScene
    {
        public IEntity Entity { get; }
        
        public List<string> Locations { get; set; } = new List<string>();
        public string DialoguePopup { get; set; }

        /// <summary>
        /// 存储当前Locations和初始对话
        /// </summary>
        /// <param name="entity"></param>
        public void Bind(IEntity entity)
        {
            SceneEntity sceneEntity = entity as SceneEntity;
            foreach (string curLocation in sceneEntity.Locations)
            {
                Locations.Add(curLocation);
            }
            DialoguePopup = sceneEntity.Dialogue;
        }

        /// <summary>
        /// LoadScene通常尤GameManager来使用
        /// 打开DialoguePopup，并初始化Locations
        /// </summary>
        public void LoadScene()
        {
            IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
            IFactoryService factoryService = ServiceProvider.Instance.GetService<IFactoryService>();
            // Spawn startup dialogue            
            UIManager.Instance.SpawnPopup(DialoguePopup);
            // Load Locations
            foreach (var locaitonId in Locations)
            {
                IEntity locationEntity = storeService.GetEntity(locaitonId);
                LocationCard card = factoryService.Create(locationEntity) as LocationCard;
                LocationDeckUI locationDeckUI = UIManager.Instance.GetLocationDeckUI();
                if (locationDeckUI != null)
                {
                    locationDeckUI.AddCard(card);
                }    
            }
            
        }
    }
}