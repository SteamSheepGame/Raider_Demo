using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Demo.Core
{
    public class BasePopup: SerializedMonoBehaviour, IPopup
    {
        // [SerializeField, Required] public GameObject PopupPrefab;
        
        protected LocationPopupEntity LocationPopupEntity;
        protected WorkTask currentTask;
        private RectTransform _rect;
        protected string PopupId;
        public RectTransform Rect => _rect ??= GetComponent<RectTransform>();
        private void OnEnable()
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
        }

        public virtual void Bind(IEntity entity)
        {
            PopupId = entity.Id;
            LocationPopupEntity = entity as LocationPopupEntity;
        }
        
        public virtual void OnPopupClosed()
        {
        }

        public virtual void StartWork()
        {
            if (currentTask != null)
            {
                currentTask.StartTask();
                currentTask.WorkTimer._OnFinished += OnWorkFinished;
            }
        }

        public virtual void OnWorkFinished()
        {
            currentTask.WorkTimer._OnFinished -= OnWorkFinished;
        }
        
        public void SetWorkTask(WorkTask task)
        {
            currentTask = task;
        }

    }
}