using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace Demo.Core
{
    public class JsonEntityParser
    {
        
        /// <summary>
        /// 返回所有带[DataImportable("tag")]的类
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Type> FindImportableTypes()
        {
            var types = new Dictionary<string, Type>();
            Assembly assembly = Assembly.GetExecutingAssembly(); // or typeof(IEntity).Assembly if needed

            foreach (var type in assembly.GetTypes())
            {
                if (!typeof(IEntity).IsAssignableFrom(type)) continue;

                var attr = type.GetCustomAttribute<DataImportable>();
                if (attr != null)
                {
                    types[attr.Tag] = type;
                }
            }

            return types;
        }
        
        /// <summary>
        /// 范类解析JSON方法
        /// </summary>
        /// <param name="entities"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<IEntity> ParseData<T>(IEnumerable<T> entities)
        {
            if (typeof(T) == typeof(LoadedData))
            {
                return ParseLoadedData(entities.Cast<LoadedData>());
            }
            
            UnityEngine.Debug.LogWarning($"[ParseHandler] Unrecognized data type: {typeof(T).Name}");
            return null;
        }
        
        /// <summary>
        /// 用LoadedData解析JSON
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private static List<IEntity> ParseLoadedData(IEnumerable<LoadedData> entities)
        {
            // 用System Reflection找到所有Entity类别
            Dictionary<string, Type> importableTypes = FindImportableTypes();
            List<IEntity> resultEntities = new List<IEntity>();
            
            foreach(LoadedData data in  entities)
            {
                if (!importableTypes.TryGetValue(data.EntityTag, out var type))
                {
                    UnityEngine.Debug.LogWarning($"[ParseHandler] Unrecognized entity type: {data.EntityTag}");
                    continue;
                }

                try
                {
                    JToken token = data.EntityContainer.Value; 
                    // Todo - Support Array in future
                    
                    if (token.Type == JTokenType.Array)
                    {
                        foreach (var child in token.Children())
                        {
                            IEntity parsedEntity = child.ToObject(type) as IEntity;
                            if (parsedEntity != null)
                                resultEntities.Add(parsedEntity);
                        }
                    }
                    else
                    {
                        IEntity parsedEntity = token.ToObject(type) as IEntity;
                        if (parsedEntity != null)
                        {
                            resultEntities.Add(parsedEntity);
                        }
                        else
                        {
                            UnityEngine.Debug.LogWarning($"[ParseHandler] Failed to parse entity of type: {type.Name}");
                        }    
                    }
                    
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[ParseHandler] Exception while parsing {type.Name}: {ex.Message}");
                }
            }

            return resultEntities;
        }
    }
    
}