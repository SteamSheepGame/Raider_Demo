using System;
using UnityEngine;

namespace Demo.Core
{
    public class TimeSystemTest : MonoBehaviour, ITimeConsumer
    {
        [SerializeField] private float lifetime = 10f;
        public Action OnExpired;
        
        public float Lifetime => lifetime;
        public string Name => gameObject.name;
        
        private void Start()
        {
            TimeSystem.Instance.RegisterTimeConsumer(this);
            
            OnExpired += () =>
            {
                Debug.Log($"{Name} has expired.");
            };
        }

        private void OnDestroy()
        {
            TimeSystem.Instance.UnregisterTimeConsumer(this);
        }
        
        public void OnLifetimeExpired()
        {
            OnExpired?.Invoke();
            Destroy(gameObject);
        }
    }
}