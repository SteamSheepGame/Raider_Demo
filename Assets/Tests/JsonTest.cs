using NUnit.Framework;
using UnityEngine;
using Demo.Core;

namespace Demo.Core
{
    public class JsonTest
    {
        [Test]
        public void ImportParseTest()
        {
            // Init DataImporter 
            DataImporter dtImporter = new DataImporter("Assets/Tests/JsonTest");
            dtImporter.LoadDataFromAssignedFolder();
            
            // Store entities inside EntityStore
            EntityStore store = new EntityStore();
            store.HandleImportData(dtImporter);
            
            // Test entity
            IEntity entity = store.GetEntity("1234");
            Assert.IsInstanceOf<CharacterEntity>(entity);
            
            CharacterEntity character = (CharacterEntity)entity;
            Assert.AreEqual(character.Aspect, "Character");
            Assert.AreEqual(character.Attributes.Charm, 3);
        }
    }    
}
