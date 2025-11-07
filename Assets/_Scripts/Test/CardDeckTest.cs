using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Demo.Core
{
    public class CardDeckTest: Singleton<CardFactoryTest>
    {
        [SerializeField] GameObject cardDeckPrefab;
        [SerializeField] GameObject cardPrefabChar;
        [SerializeField] GameObject cardPrefabLoc;
        [SerializeField] GameObject locationPopup;
        [SerializeField] GameObject dialoguePopup;
        [SerializeField] GameObject cardSlotPrefab;
        [SerializeField] GameObject characterPopup;
        [SerializeField] GameObject questPopupPrefab;

        private List<CharacterCard> cards;
        
        protected override void Initialize()
        {
           
        }

        private void Start()
        {
            cards = new List<CharacterCard>();
            
            // Init DataImporter 
            DataImporter dtImporter = new DataImporter("Assets/Tests/JsonTest");
            dtImporter.LoadDataFromAssignedFolder();
            // Store entities inside EntityStore
            ServiceProvider.Instance.RegisterService<IEntityStoreService>(new EntityStoreService());
            ServiceProvider.Instance.RegisterService<IActionService>(new ActionService());
            IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
            storeService.HandleImportData(dtImporter);
            
            // Init FactoryService
            ServiceProvider.Instance.RegisterService<IFactoryService>(new FactoryService());
            IFactoryService factoryService = ServiceProvider.Instance.GetService<IFactoryService>();
            
            //Register Factory
            factoryService.Register<CharacterEntity>(new CharacterCardFactory(cardPrefabChar));
            factoryService.Register<LocationEntity>(new LocationCardFactory(cardPrefabLoc));
            factoryService.Register<LocationPopupEntity>(new LocationPopupFactory(locationPopup));
            factoryService.Register<CharacterSlotEntity>(new CharacterSlotFactory(cardSlotPrefab));
            factoryService.Register<DialoguePopupEntity>(new DialoguePopupFactory(dialoguePopup));
            factoryService.Register<CharacterPopupEntity>(new CharacterPopupFactory(characterPopup));
            factoryService.Register<QuestboardPopupEntity>(new QuestboardPopupFactory(questPopupPrefab));
           
            foreach (IEntity entity in storeService.GetAllEntities())
            {
                if (entity is CharacterEntity)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        CharacterCard card = factoryService.Create(entity) as CharacterCard;
                        cards.Add(card);
                    }
                } else if (entity is LocationEntity)
                {
                    LocationCard card = factoryService.Create(entity) as LocationCard;
                    LocationDeckUI locationDeckUI = UIManager.Instance.GetLocationDeckUI();
                    if (locationDeckUI != null)
                    {
                        locationDeckUI.AddCard(card);
                    }
                } else if (entity is CharacterSlotEntity)
                {
                    
                }
                else
                {
                    Debug.Log(entity.Id);
                }
            }
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