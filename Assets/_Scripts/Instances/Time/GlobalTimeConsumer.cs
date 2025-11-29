using UnityEngine;
namespace Demo.Core
{
    public class GlobalTimeConsumer: MonoBehaviour, ITimeConsumer
    {
        [SerializeField] private float lifetime = 999999f;
        [SerializeField] private float GlobalTimeSpeed = 20.0f;
        public string Name => "GlobalTimeConsumer";
        public float Lifetime => lifetime;

        private void Start()
        {
            TimeSystem.Instance.RegisterTimeConsumer(this);
            TimeSystem.Instance.SetTimeScale(GlobalTimeSpeed);
        }

        public void OnLifetimeExpired()
        {
            // Never fires because lifetime is huge
        }
    }
}