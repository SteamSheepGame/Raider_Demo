using System.Collections.Generic;
namespace Demo.Core
{
    [DataImportable("LocationPopup")]
    public class LocationPopupEntity: IEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Characters { get; set; }
        public List<string> Slots { get; set; }
    }
}