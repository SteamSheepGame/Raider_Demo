using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEditor.Graphs;
using UnityEngine.EventSystems;

namespace Demo.Core
{
    public class BaseCard<TCard>: SerializedMonoBehaviour, ICard, 
        IBeginDragHandler,  IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler where TCard : BaseCard<TCard>
    {
        
        [TitleGroup("UI References")]
        [SerializeField, Required] private Image _backgroundImage;
        [SerializeField, Required] private TextMeshProUGUI _titleText;
        [SerializeField, Required] private RectTransform _rect;
        // [SerializeField, Required] private CharacterSlot _characterSlot;
        
        [TitleGroup("UI Setting")]
        [SerializeField] private float StartingAlpha = 1f;
        [SerializeField] private float SelectedAlpha = 0.5f;
        
        public event Action<BaseCard<TCard>> OnClicked;
        public IEntity Entity { get; protected set;}

        public ISlot OccupiedSlot { get; private set; }
        
        public IDeck<TCard> ParentDeck { get; private set; }
        
        // During drag
        private ISlot _originSlot;
        private ISlot _highlightedSlot;

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
        public bool IsClickable { get; set; } = true;
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
        public virtual void Bind(IEntity entity)
        {
            // // 链接Entity
            // Entity = entity;
            // CharacterEntity Character = entity as CharacterEntity;
            //
            // // 初始化卡片UI
            // Label = Character.Label;
            // Background = TryLoadSpriteFromResources(Character.Image);
        }
        
        protected static Sprite TryLoadSpriteFromResources(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            var sprite = Resources.Load<Sprite>(path);
            if (!sprite)
                Debug.LogWarning($"[CharacterCardView] Sprite not found at Resources path: {path}");
            return sprite;
        }
        
        public void SetParentDeck(IDeck deck)
        {
            ParentDeck = deck as IDeck<TCard>;
            OccupiedSlot = null;
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

        /*public void MoveTo(Vector3 worldPos, float duration = 0.15f)
        {
            this.transform.localPosition = worldPos;
        }*/

        public void PlaceCard(ISlot slot)
        {
            Rect.SetParent(slot.Rect);
            Rect.anchoredPosition = Vector2.zero;
            OccupiedSlot = slot;
            //Rect.anchoredPosition = OccupiedSlot.Rect.anchoredPosition;
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
            if(!IsDraggable)  return;
            // 调整Alpha
            _canvasGroup.alpha = SelectedAlpha;
            // 
            _originSlot = OccupiedSlot;

            // 从Slot中移出
            if (ParentDeck != null)
            {
                ParentDeck.TryRemove(this as TCard);
            }
            else if (_originSlot != null)
            {
                _originSlot.Clear();
                OccupiedSlot = null;
            }

            // 暂时放入Canvas，以后可能加入其他layer专门放置移动中的卡牌
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, Rect.position);
            RectTransform canvasRect = UIManager.Instance.HUDView.Rect;
            Rect.SetParent(canvasRect, false);
            // 从Local位置转为World
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, null, out localPos);
            Rect.anchoredPosition = localPos;
        }

        void UnityEngine.EventSystems.IDragHandler.OnDrag(PointerEventData Data)
        {
            if(!IsDraggable)  return;
            // 让Card跟住鼠标
            Rect.anchoredPosition += Data.delta / _canvas.scaleFactor;
            // 检查是否碰到Slot
            ISlot newSlot = SlotManager.Instance?.GetNearestOverlapping(Rect);
            // 如果碰到slot是上次Hightlight过的，结束
            if (ReferenceEquals(newSlot, _highlightedSlot)) return;
            // 不是上次highlight过的，结束highlight
            if (_highlightedSlot != null)
            {
                _highlightedSlot.Highlight(false);
            }
            _highlightedSlot = newSlot;
            // Highlight新的
            if (_highlightedSlot != null)
            {
                _highlightedSlot.Highlight(true);
            }
        }

        void UnityEngine.EventSystems.IEndDragHandler.OnEndDrag(PointerEventData Data)
        {
            if(!IsDraggable)  return;
            // 重置Alpha值
            _canvasGroup.alpha = StartingAlpha;
            
            // 尝试插入slot
            ISlot overlappedSlot = SlotManager.Instance?.GetNearestOverlapping(Rect); 
            if (overlappedSlot != null && overlappedSlot.TryAccept(this))
            {
                OccupiedSlot = overlappedSlot;
                return;
            }
            
            _highlightedSlot?.Highlight(false);
            _highlightedSlot = null;
            
            // 插入失败, 清除Occupied Slot
            OccupiedSlot?.Clear();
            OccupiedSlot = null;
            // 返回Deck
            if (ParentDeck != null)
            {
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(ParentDeck.Rect, Data.position, null, out localPos);
                int insertIndex = ParentDeck.GetInsertIndexFromLocalPosition(localPos);
                ParentDeck.TryInsert(this as TCard, insertIndex);
            }
        }

        #endregion
        
        #region Click
        
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if(!IsClickable)  return;
            Select(true);
            
            OnClicked?.Invoke(this);
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if(!IsClickable)  return;
            Select(false);
        }
        #endregion

        #region Hover

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!IsClickable)  return;
            Highlight(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(!IsClickable)  return;
            Highlight(false);
        }

        #endregion
    }
}