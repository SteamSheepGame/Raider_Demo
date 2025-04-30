using System.IO;
using LitJson;
using UnityEngine;

namespace Demo.Core
{
    /// <summary>
    /// 读取和保存Json文件的服务类
    /// 注意：目前没有实现版本号检测控制
    /// </summary>
    public class JsonService : IJsonService
    {
        /// <summary>
        /// 保存Json数据，保存至持久化数据路径
        /// </summary>
        /// <param name="fileName">要保存的文件名</param>
        /// <param name="data">要保存的数据实例</param>
        /// <typeparam name="T">要保存的数据类</typeparam>
        public void SaveJson<T>(string fileName, T data) where T : new()
        {
            //默认尝试写入持久化数据路径
            string path = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
            string json = JsonMapper.ToJson(data);
            File.WriteAllText(path, json);
            
            Debug.Log("[JsonService] [SaveJson] 成功保存数据: " + path);  
        }

        /// <summary>
        /// 读取Json数据，优先读取持久化数据路径，否则拷贝StreamingAssets路径数据至持久化数据路径
        /// </summary>
        /// <param name="fileName">要读取的文件名</param>
        /// <typeparam name="T">要读取的数据类</typeparam>
        /// <returns></returns>
        public T LoadJson<T>(string fileName) where T : new()
        {
            //默认尝试读取持久化数据路径
            string path = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

            //如果持久化数据路径不存在，则尝试读取StreamingAssets路径
            if (!File.Exists(path))
            {
                string streamingPath = Path.Combine(Application.streamingAssetsPath, $"{fileName}.json");
                if (!File.Exists(streamingPath))
                {
                    Debug.LogError("[JsonService] [LoadJson] 文件不存在。");
                    return new T();
                }
                //将数据复制入持久化数据路径
                string json = File.ReadAllText(streamingPath);
                File.WriteAllText(path, json);
            }
            
            //读取持久化数据路径
            string finalJson = File.ReadAllText(path);
            
            T data = JsonMapper.ToObject<T>(finalJson);
            Debug.Log("[JsonService] [LoadJson] 成功读取数据: " + path);
            
            return data;
        }
    }
}
