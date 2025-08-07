using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Demo.Core
{
    /// <summary>
    /// 用于管理所有的实体存储库的顶层
    /// </summary>
    public class Compendium : Singleton<Compendium>
    {
        /// <summary>
        /// Key: 类名；Value: 实体存储库。例如 Element ： ElementStore
        /// </summary>
        [OdinSerialize] [ReadOnly] private Dictionary<Type, EntityStoreService> _stores;
        
        public void InitializeEntityData(IEnumerable<Type> dataTypes)
        {
            _stores = new Dictionary<Type, EntityStoreService>();

            foreach (Type dataType in dataTypes)
            {
                if (dataType.IsAssignableFrom(typeof(IEntity)))
                {
                   _stores.Add(dataType, new EntityStoreService());
                }
            }
        }
    }
}