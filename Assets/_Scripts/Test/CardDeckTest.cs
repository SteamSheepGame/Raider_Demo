using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    public class CardDeckTest: Singleton<CardFactoryTest>
    {
        [SerializeField] GameObject cardDeckPrefab;
        [SerializeField] GameObject cardPrefab;

        private List<CharacterCard> cards;
        protected override void Initialize()
        {
           
            cards = new List<CharacterCard>();
            
            // Init DataImporter 
            DataImporter dtImporter = new DataImporter("Assets/Tests/JsonTest");
            dtImporter.LoadDataFromAssignedFolder();
            // Store entities inside EntityStore
            ServiceProvider.Instance.RegisterService<IEntityStoreService>(new EntityStoreService());
            IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
            storeService.HandleImportData(dtImporter);
            
            // Init FactoryService
            ServiceProvider.Instance.RegisterService<IFactoryService>(new FactoryService());
            IFactoryService factoryService = ServiceProvider.Instance.GetService<IFactoryService>();

            //Register Factory
            factoryService.Register<CharacterEntity>(new CharacterCardFactory(cardPrefab));
           
            foreach (IEntity entity in storeService.GetAllEntities())
            {
                if (entity is CharacterEntity)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        CharacterCard card = factoryService.Create(entity) as CharacterCard;
                        cards.Add(card);
                    }
                }
            }
        }

        private void Start()
        {
            // Init Player Deck
            PlayerDeckManager.Instance.InitPlayerDeck();
            CharacterDeck Deck = PlayerDeckManager.Instance.Get<CharacterDeck>();
            if (Deck != null)
            {
                Deck.InitDeck(cards);
            }
        }
    }
}