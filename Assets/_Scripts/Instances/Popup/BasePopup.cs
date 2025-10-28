using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Demo.Core
{
    public class BasePopup: SerializedMonoBehaviour, IPopup
    {
        [SerializeField, Required] public GameObject PopupPrefab;
        
        protected PopupEntity PopupEntity;
        
        private RectTransform _rect;
        public RectTransform Rect => _rect ??= GetComponent<RectTransform>();
        private void OnEnable()
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
        }

        public virtual void Bind(IEntity entity)
        {
            PopupEntity = entity as PopupEntity;
        }

    }
}