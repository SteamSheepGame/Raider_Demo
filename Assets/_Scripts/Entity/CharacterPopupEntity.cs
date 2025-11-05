namespace Demo.Core
{
    [DataImportable("CharacterPopup")]
    public class CharacterPopupEntity: IEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}