using System;
using UnityEngine;
namespace Demo.Core
{
    // Simplified for testing purposes
    public class CardFactoryTest: Singleton<CardFactoryTest>
    {
        [SerializeField] GameObject cardPrefab; 
        
        protected override void Initialize()
        {
            // Init DataImporter 
            DataImporter dtImporter = new DataImporter("Assets/Tests/JsonTest");
            dtImporter.LoadDataFromAssignedFolder();
            
            // Store entities inside EntityStore
            ServiceProvider.Instance.RegisterService<IEntityStoreService>(new EntityStoreService());
            IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
            storeService.HandleImportData(dtImporter);
            
            // Init FactoryRegistry
            CardFactoryRegistry.Register<CharacterEntity>(new CharacterCardFactory(cardPrefab));
            // Create Card for each entity in 
            foreach (IEntity entity in storeService.GetAllEntities())
            {
                if (entity is CharacterEntity)
                {
                    var card = CardFactoryRegistry.CreateCard(entity);
                }
            }
        }
        
    }
}