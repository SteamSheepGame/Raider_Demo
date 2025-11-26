using System;
using System.Collections.Generic;
namespace Demo.Core
{
    [DataImportable("Scenes")]
    public class SceneEntity: IEntity
    {
        public string Id { get; set; }
        public string Dialogue { get; set; }
        public string[] Locations { get; set; }
    }
}