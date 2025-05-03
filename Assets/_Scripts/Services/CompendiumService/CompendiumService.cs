using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Demo.Core
{
    /// <summary>
    /// 用于从数据文件夹加载数据
    /// </summary>
    public class CompendiumService : ICompendiumService
    {
        private readonly string _contentFolderName;
        
        public CompendiumService(string contentFolderName)
        {
            _contentFolderName = contentFolderName;
        }
        
        public void PopulateCompendium(Compendium compendium)
        {
            //获取存放数据的文件夹名称
            string contentFilePath = Path.Combine(Application.streamingAssetsPath, _contentFolderName);
            //获取当前程序集
            Assembly assembly = Assembly.GetAssembly(GetType());
            //存放可导入数据类型的列表
            List<Type> importableDataTypes = new List<Type>();
            
            //导入数据
            DataImporter contentFileImporter = new DataImporter(contentFilePath);
            contentFileImporter.LoadDataFromAssignedFolder();
            //将所有可导入的类型加入列表
            foreach (Type type in assembly.GetTypes())
            {
                DataImportable importableData = type.GetCustomAttribute(typeof(DataImportable), false) as DataImportable;
                if (importableData == null)
                    continue;
                
                importableDataTypes.Add(type);
            }
            //从所有可导入类中找出实体类并注册入Compendium，生成EntityStore
            compendium.InitializeEntityData(importableDataTypes);
            
            
        }
    }
}