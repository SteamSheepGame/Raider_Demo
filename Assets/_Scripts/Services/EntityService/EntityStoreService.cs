using System.Collections.Generic;
using PlasticGui.Configuration.CloudEdition;

namespace Demo.Core
{
    /// <summary>
    /// 用于管理特定类所有的实体的存储库
    /// </summary>
    public class EntityStoreService : IEntityStoreService
    {
        /// <summary>
        /// Key: 实体ID; Value: 实体数据。 例如：“health”：ElementEntity(health)
        /// </summary>
        private Dictionary<string, IEntity> _entities = new Dictionary<string, IEntity>();

        
        /// <summary>
        /// 利用DataImporter传入的数据获取Entity
        /// </summary>
        /// <param name="importer"></param>
        public void HandleImportData(DataImporter importer)
        {
            List<IEntity> parsedData = importer.HandleLoadedData();
            foreach (IEntity input in parsedData)
            {
                _entities.Add(input.Id, input);
            }
        }
        
        /// <summary>
        /// 添加Entity
        /// </summary>
        /// <param name="entity"></param>
        public void PushEntity(IEntity entity)
        {
           _entities.Add(entity.Id, entity); 
        }
        
        /// <summary>
        /// 利用id获取Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEntity GetEntity(string id)
        {
            return _entities[id];
        }
        
        /// <summary>
        /// 返回全部Entity
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IEntity> GetAllEntities()
        {
            return _entities.Values;
        }
    }
}