using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Demo.Core
{
    public class DataImporter
    {
        private readonly string _folderPath;

        private List<string> _contentFilePaths = new List<string>();
        private readonly List<LoadedData> _loadedData = new List<LoadedData>();
        
        public DataImporter(string path)
        {
            _folderPath = path;
        }

        /// <summary>
        /// 获取路径下所有的Json文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<string> GetContentFiles(string path)
        {
            List<string> contentFilePaths = new List<string>();
            if(Directory.Exists(_folderPath))
            {
                contentFilePaths.AddRange(
                    Directory.GetFiles(path).ToList().FindAll(file=>file.EndsWith(".json")));
                foreach (var subdirectory in Directory.GetDirectories(path))
                {
                    contentFilePaths.AddRange(GetContentFiles(subdirectory));
                }
            }
            return contentFilePaths;
        }

        /// <summary>
        /// 将文件夹下的所有Json文件加载到List中
        /// </summary>
        public void LoadDataFromAssignedFolder()
        {
            _contentFilePaths = GetContentFiles(_folderPath);

            if (_contentFilePaths.Any())
            {
                _contentFilePaths.Sort();
            }

            foreach (string contentFilePath in _contentFilePaths)
            {
                long length = new FileInfo(contentFilePath).Length;
                if (length < 8) //允许我们忽略空文件，这在Steam占位符中特别有用
                    continue;

                try
                {
                    using StreamReader file = File.OpenText(contentFilePath);
                    using JsonTextReader reader = new JsonTextReader(file);
                    JObject topLevelObject = (JObject)JToken.ReadFrom(reader);
                    
                    // 读取第一个属性作为容器属性(如"elements"或"recipes")
                    JProperty containerProperty = topLevelObject.Properties().First();
                    LoadedData loadedData = new LoadedData(contentFilePath, containerProperty, containerProperty.Name);
                    _loadedData.Add(loadedData);
                }
                catch(Exception e)
                {
                    Debug.LogError($"加载数据出现问题{e.Message}，文件路径：{contentFilePath}");
                }
            }
        }
        
        /// <summary>
        /// 利用JsonEntityParser解析json数据
        /// </summary>
        /// <returns></returns>
        public List<IEntity> HandleLoadedData()
        {
            List<IEntity> entities = JsonEntityParser.ParseData(_loadedData) as List<IEntity>;
            return entities;
        }
    }
}