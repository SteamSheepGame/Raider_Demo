using UnityEngine;
namespace Demo.Core
{
    public class WorkTimeConsumer: ITimeConsumer
    {
        public string Name { get; private set; }
        public float Lifetime { get; private set; }

        public void Init(string name, float lifetime)
        {
            Name = name;
            Lifetime = lifetime;
            TimeSystem.Instance.RegisterTimeConsumer(this);
        }
        
        public void OnLifetimeExpired()
        {
            TimeSystem.Instance.UnregisterTimeConsumer(this);
        }
    }
}