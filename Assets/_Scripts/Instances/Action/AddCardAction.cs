namespace Demo.Core
{
    public class AddCardAction: BaseActionHandler
    {
        public override void Execute(ActionBinding action)
        {
            string CharacterCard = GetParam(action, "Character");
            
            IEntityStoreService storeService = ServiceProvider.Instance.GetService<IEntityStoreService>();
            IFactoryService factoryService = ServiceProvider.Instance.GetService<IFactoryService>();
            
            CharacterEntity CharacterCardEntity = storeService.GetEntity(CharacterCard) as CharacterEntity;
            if (CharacterCardEntity != null)
            {
                CharacterCard card = factoryService.Create(CharacterCardEntity) as CharacterCard;
                CharacterDeck Deck = PlayerDeckManager.Instance.Get<CharacterDeck>();
                Deck.TryAdd(card);
            }
        }
    }
}