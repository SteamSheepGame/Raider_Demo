using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    [DataImportable("action")]
    public class ActionEntity: IEntity
    {
        public string Id { get; set; }
        public string OpenPopup { get; set; }
        public string PopupId { get; set; }
        public string TargetType;
    }
}