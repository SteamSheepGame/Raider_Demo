using System;
using System.Collections.Generic;
namespace Demo.Core
{
    [DataImportable("character")]
    public class CharacterEntity : IEntity
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Aspect { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public CharacterAttributes Attributes { get; set; }
        public List<ActionBinding> AvailableActions { get; set; } = new();
    }

    [Serializable]
    public struct CharacterAttributes
    {
        public int Intellect;
        public int Religion;
        public int Strength;
        public int Charm;
        public int Occult;
        public int History;
        public int Technology;
        public int Sanity;

        public int this[string name]
        {
            get
            {
                return name.ToLower() switch
                {
                    "intellect" => Intellect,
                    "religion" => Religion,
                    "strength" => Strength,
                    "charm" => Charm,
                    "occult" => Occult,
                    "history" => History,
                    "technology" => Technology,
                    "sanity" => Sanity,
                    _ => throw new ArgumentException($"Unknown attribute: {name}")
                };
            }
        }
    }
}