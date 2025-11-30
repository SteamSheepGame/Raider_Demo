using System;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using FilePathAttribute = UnityEditor.FilePathAttribute;
using Object = UnityEngine.Object;

namespace Demo.Core
{
    /// <summary>
    /// 一个卡牌堆UI模组
    /// </summary>
    public class LocationDeckUI : SerializedMonoBehaviour
    {
        [TitleGroup("UI References")]
        [SerializeField] private RectTransform _rect;       
        
        [TitleGroup("Settings")]
        [SerializeField] private float _yOffset = 20.0f;     
        
        private readonly List<LocationCard> _spawnedCards = new();

        private void OnEnable()
        {
            _spawnedCards.Clear();
        }

        public void AddCard(LocationCard card)
        {
            if (card == null)
            {
                Debug.Log("Yo where is the card?");
            }
            
            card.Rect.SetParent(_rect, false);
            // int curSize = _spawnedCards.Count;
            // Vector2 pos = new Vector2(0f, -curSize * _yOffset); // top-down stacking
            // card.Rect.anchoredPosition = pos;
            
            _spawnedCards.Add(card);
        }

        public void ClearCards()
        {
            for (int i = _spawnedCards.Count - 1; i >= 0; i--)
            {
                var card = _spawnedCards[i];
                if (card != null && card.Rect != null)
                {
                    // Destroy the GameObject, not just remove from list
                    Object.Destroy(card.Rect.gameObject);
                }
            }

            _spawnedCards.Clear();
        }
    }
}