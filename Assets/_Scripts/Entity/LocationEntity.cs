using System;
using System.Collections.Generic;

namespace Demo.Core
{
    [DataImportable("location")]
    public class LocationEntity: IEntity
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Description{ get; set; }
        public string Image { get; set; }

        public string Slots { get; set; }
        public List<ActionBinding> AvailableActions { get; set; } = new();
    }
    
    [Serializable]
    public class ActionBinding
    {
        public string Id { get; set; }               // Unique ID for this binding
        public string Trigger { get; set; }          // e.g. "OnClick", "OnHover", "OnEnter"
        public Dictionary<string, string> Params;
    }
}