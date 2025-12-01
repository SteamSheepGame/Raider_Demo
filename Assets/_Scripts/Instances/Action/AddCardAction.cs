

namespace Demo.Core
{
    public class AddCardAction: BaseActionHandler
    {
        public override void Execute(ActionBinding action)
        {
            // TODO: THIS IS BAD!
            string CharacterCard = GetParam(action, "Character");
            string ItemCard = GetParam(action, "Item");
            
            IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
            IFactoryService factoryService = ServiceProvider.Instance.GetService<IFactoryService>();
            
            CharacterEntity CharacterCardEntity = storeService.GetEntity(CharacterCard) as CharacterEntity;

            CharacterEntity ItemCardEntity = null;
            if(ItemCard != "") ItemCardEntity = storeService.GetEntity(ItemCard) as CharacterEntity;
            
            CharacterDeck Deck = PlayerDeckManager.Instance.Get<CharacterDeck>();
            if (CharacterCardEntity != null)
            {
                CharacterCard card = factoryService.Create(CharacterCardEntity) as CharacterCard;
                Deck.TryAdd(card);
            }
            
            if (ItemCardEntity != null)
            {
                CharacterCard card = factoryService.Create(ItemCardEntity) as CharacterCard;
                Deck.TryAdd(card);
            }
        }
    }
}