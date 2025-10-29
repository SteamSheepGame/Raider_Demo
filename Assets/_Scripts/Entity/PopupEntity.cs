using System.Collections.Generic;
namespace Demo.Core
{
    [DataImportable("popup")]
    public class PopupEntity: IEntity
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Characters { get; set; }
    }
}