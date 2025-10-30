using System.Collections.Generic;
namespace Demo.Core
{
    [DataImportable("CharacterSlot")]
    public class CharacterSlotEntity: IEntity
    { 
        public string Id { get; set; }
        public string Type { get; set; }
        public List<ActionBinding> AvailableActions { get; set; } = new();
    }
}