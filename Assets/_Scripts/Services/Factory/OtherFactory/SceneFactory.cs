using UnityEngine;
namespace Demo.Core
{
    public class SceneFactory: Factory<IScene>
    {
        public SceneFactory(GameObject prefab) : base(prefab)
        {
        }

        protected override IScene CreateInstance(IEntity entity = null)
        {
            if (entity == null)
            {
                Debug.Log("Creating new instance of SceneEntity, but entity is null");
            }
            
            SceneEntity sceneEntity = entity as SceneEntity;
            if (sceneEntity == null)
            {
                Debug.Log("SceneFactory::CreateInstance: entity is not SceneEntity");
                return null;
            }
            
            Scene sceneInstance = new Scene();
            sceneInstance.Bind(sceneEntity);

            return sceneInstance;
        }
    }
}