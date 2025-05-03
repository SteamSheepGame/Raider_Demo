using System.Collections.Generic;

namespace Demo.Core
{
    /// <summary>
    /// 用于管理特定类所有的实体的存储库
    /// </summary>
    public class EntityStore
    {
        /// <summary>
        /// Key: 实体ID; Value: 实体数据。 例如：“health”：ElementEntity(health)
        /// </summary>
        private Dictionary<string, IEntity> _entities = new Dictionary<string, IEntity>();
    }
}