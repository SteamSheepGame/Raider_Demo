using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo.Core
{
    /// <summary>
    /// DeckManager存在是为了handle以后放卡的功能，找到不同的Deck
    /// </summary>
    public class PlayerDeckManager: Singleton<PlayerDeckManager>
    {
        public enum DeckKind { Event, Character, Item, Abstract }
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
            // 通过UIManager加入View
            Transform parentCanvas = UIManager.Instance.GUIView.transform;
            var deckObject = Object.Instantiate(_deckPrefabMap[DeckKind.Character], parentCanvas);
            deckObject.name = "Character_Deck";
            UIManager.Instance.GUIView.AddPopup(deckObject, deckObject.name);
            UIManager.Instance.ClosePopup(deckObject.name);
            
            CharacterDeck deck = deckObject.GetComponent<CharacterDeck>();
            // Set Position for Character Deck
            var pos = deck.Rect.anchoredPosition;
            pos = parent.anchoredPosition;
            deck.Rect.anchoredPosition = pos;
            
            return deck;
        }
    }
}