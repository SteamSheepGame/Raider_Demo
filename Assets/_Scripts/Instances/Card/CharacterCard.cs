using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;         
using TMPro;
using UnityEngine.EventSystems;

namespace Demo.Core
{
    public class CharacterCard: SerializedMonoBehaviour, ICard, 
        IBeginDragHandler,  IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        [TitleGroup("UI References")]
        [SerializeField, Required] private Image backgroundImage;
        [SerializeField, Required] private TextMeshProUGUI titleText;
        [SerializeField, Required] private RectTransform rect;
        
        [TitleGroup("UI Setting")]
        [SerializeField] private float StartingAlpha = 1f;
        [SerializeField] private float SelectedAlpha = 0.5f;
        public IEntity Entity { get; private set;}
        
        // Visuals
        public Sprite Background
        {
            get => backgroundImage != null ? backgroundImage.sprite : null;
            set
            {
                if (backgroundImage != null) backgroundImage.sprite = value;
            }
        }

        public string Label
        {
            get => titleText != null? titleText.text : null;
            set
            {
                if (titleText != null) titleText.text = value;
            }
        }

        public RectTransform Rect
        {
            get => rect != null? rect : null;
            set
            {
                if (rect != null) rect = value;
            }
        }
        
        private Canvas canvas;
        private CanvasGroup canvasGroup;
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
            canvas = GetComponentInParent<Canvas>();       // needed for scaling
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
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
            canvasGroup.alpha = SelectedAlpha;
            // 从卡堆里移除
        }

        void UnityEngine.EventSystems.IDragHandler.OnDrag(PointerEventData Data)
        {
            Rect.anchoredPosition += Data.delta / canvas.scaleFactor;
        }

        void UnityEngine.EventSystems.IEndDragHandler.OnEndDrag(PointerEventData Data)
        {
            // 调整透明度
            canvasGroup.alpha = StartingAlpha;
            // 如果没有进slot，返回卡堆
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