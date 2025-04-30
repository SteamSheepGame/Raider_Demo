using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Demo.Core
{
    public class JsonServiceTest : SerializedMonoBehaviour
    {
        
        private const string JSON_FILE_NAME = "Elements";

        [BoxGroup("数据")] [OdinSerialize] 
        private string _id;
        
        [BoxGroup("数据")] [OdinSerialize] 
        private string _description;
        
        [BoxGroup("数据")] [OdinSerialize] 
        private List<string> _tags;
        
        [BoxGroup("数据")] [OdinSerialize]
        private Dictionary<string, int> _aspects;
        
        void Start()
        {
            IJsonService jsonService = new JsonService();
            ServiceProvider.Instance.RegisterService<IJsonService>(jsonService);
        }
        

        [Button("SaveJson")]
        void SaveJson()
        {
            TestData newData = GetTestData();
            IJsonService jsonService = ServiceProvider.Instance.GetService<IJsonService>();
            
            jsonService.SaveJson<TestData>(JSON_FILE_NAME, newData);
        }

        [Button("LoadJson")]
        void LoadJson()
        {
            IJsonService jsonService = ServiceProvider.Instance.GetService<IJsonService>();
            
            TestData data = jsonService.LoadJson<TestData>(JSON_FILE_NAME);
            
            Debug.Log("获取到数据: " + data.ID + " " + data.Description);
        }
        
        private TestData GetTestData()
        {
            TestData data = new TestData(_id, _description, _tags, _aspects);
            return data;
        }
    }
    
    public class TestData
    {
        public readonly string ID;
        public readonly string Description;
        public List<string> Tags;
        public Dictionary<string, int> Aspects;
        
        public TestData()
        {
        }
        
        public TestData(string id, string description, List<string> tags, Dictionary<string, int> aspects)
        {
            ID = id;
            Description = description;
            Tags = tags;
            Aspects = aspects;
        }
    }
}
