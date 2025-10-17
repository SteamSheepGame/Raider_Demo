using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo.Core
{
    public class PlayerDeckManager: Singleton<PlayerDeckManager>
    {
        public enum DeckKind { Play, Character, Item, Lore, Skill }
        [System.Serializable]
        public struct DeckPrefab
        {
            public DeckKind kind;
            public GameObject prefab;   // prefab has the correct deck component on it
        }
        
        [SerializeField] RectTransform parent;      // UI canvas/holder
        [SerializeField] DeckPrefab[] decks;    // assign in Inspector
        
        private readonly Dictionary<DeckKind, GameObject> _deckPrefabMap = new();
        private readonly Dictionary<Type, object> _deckMap = new();

        protected override void Initialize()
        {
            foreach (var d in decks)
                _deckPrefabMap[d.kind] = d.prefab;
        }
        
        public TDeck Get<TDeck>()
        {
            if (_deckMap.TryGetValue(typeof(TDeck), out var deck))
                return (TDeck)deck;

            Debug.LogError($"Deck of type {typeof(TDeck).Name} not found.");
            return default;
        }

        public void InitPlayerDeck()
        {
            _deckMap[typeof(CharacterDeck)] = CreateCharacterDeck();
        }
        
        

        protected CharacterDeck CreateCharacterDeck()
        {
            Transform parentCanvas = UIManager.Instance.HUDView.transform;
            var deckObject = Object.Instantiate(_deckPrefabMap[DeckKind.Character], parentCanvas);
            
            CharacterDeck deck = deckObject.GetComponent<CharacterDeck>();
            // Set Position for Character Deck
            var pos = deck.Rect.anchoredPosition;
            pos = parent.anchoredPosition;
            deck.Rect.anchoredPosition = pos;
            
            return deck;
        }
    }
}