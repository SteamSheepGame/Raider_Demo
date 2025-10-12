using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Graphs;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Demo.Core
{
    public class CharacterCard: SerializedMonoBehaviour, ICard, 
        IBeginDragHandler,  IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        
        [TitleGroup("UI References")]
        [SerializeField, Required] private Image _backgroundImage;
        [SerializeField, Required] private TextMeshProUGUI _titleText;
        [SerializeField, Required] private RectTransform _rect;
        
        [TitleGroup("UI Setting")]
        [SerializeField] private float StartingAlpha = 1f;
        [SerializeField] private float SelectedAlpha = 0.5f;
        public IEntity Entity { get; private set;}
        
        public ISlot OccupiedSlot { get; private set; }
        
        // Visuals
        public Sprite Background
        {
            get => _backgroundImage != null ? _backgroundImage.sprite : null;
            set
            {
                if (_backgroundImage != null) _backgroundImage.sprite = value;
            }
        }

        public string Label
        {
            get => _titleText != null? _titleText.text : null;
            set
            {
                if (_titleText != null) _titleText.text = value;
            }
        }

        public RectTransform Rect
        {
            get => _rect != null? _rect : null;
            set
            {
                if (_rect != null) _rect = value;
            }
        }
        
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        // State
        public bool IsSelected { get; set; }
        public bool IsDraggable { get; set; } = true;
        public bool IsFaceUp { get; set; } = true;
        // Events
        public event Action<ICard> Clicked;
        public event Action<ICard> BeginDrag;
        public event Action<ICard> EndDrag;
        public event Action<ICard, ISlot> DroppedOnSlot;
        
        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();       // needed for scaling
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        /// <summary>
        /// 与entity类链接
        /// </summary>
        /// <param name="entity"></param>
        public void Bind(IEntity entity)
        {
            // 链接Entity
            Entity = entity;
            CharacterEntity Character = entity as CharacterEntity;
            
            // 初始化卡片UI
            Label = Character.Label;
            Background = TryLoadSpriteFromResources(Character.Image);
        }
        
        private static Sprite TryLoadSpriteFromResources(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            var sprite = Resources.Load<Sprite>(path);
            if (!sprite)
                Debug.LogWarning($"[CharacterCardView] Sprite not found at Resources path: {path}");
            return sprite;
        }
        
        
        /// <summary>
        /// 扩展到细节面板
        /// </summary>
        public void Expand()
        {
            
        }

        public void Highlight(bool on)
        {
            IsSelected = on;
            Debug.Log($"{name}'s highlight status is  $\"{on}");
        }

        public void MoveTo(Vector3 worldPos, float duration = 0.15f)
        {
            this.transform.localPosition = worldPos;
        }

        public void SnapTo(ISlot slot)
        {
            
        }

        public void Select(bool on)
        {
            
        }

        #region Drag

        void UnityEngine.EventSystems.IBeginDragHandler.OnBeginDrag(PointerEventData Data)
        {
            // 调整透明度
            _canvasGroup.alpha = SelectedAlpha;
            // 从卡堆里移除
        }

        void UnityEngine.EventSystems.IDragHandler.OnDrag(PointerEventData Data)
        {
            Rect.anchoredPosition += Data.delta / _canvas.scaleFactor;
        }

        void UnityEngine.EventSystems.IEndDragHandler.OnEndDrag(PointerEventData Data)
        {
            // 调整透明度
            _canvasGroup.alpha = StartingAlpha;
            
            bool inSlot = false;

            ISlot OverlappedSlot = SlotManager.Instance?.GetNearestOverlapping(Rect);

            if (OverlappedSlot != null)
            {
                inSlot = OverlappedSlot.TryAccept(this);
                if (!inSlot)
                {
                    // 没有进slot，返回卡堆
                    transform.localPosition = Vector3.zero;
                    Debug.Log("未放入slot，返回卡堆");
                    if (OccupiedSlot != null)
                    {
                        OccupiedSlot.Clear();
                    }
                }
                else
                {
                    OccupiedSlot = OverlappedSlot;
                }
            }
            else
            {
                transform.localPosition = Vector3.zero;
                Debug.Log("未放入slot，返回卡堆");
            }
        }

        #endregion
        
        #region Click
        
        public void OnPointerDown(PointerEventData eventData)
        {
            Select(true);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            Select(false);
        }
        #endregion

        #region Hover

        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Highlight(false);
        }

        #endregion
    }
}