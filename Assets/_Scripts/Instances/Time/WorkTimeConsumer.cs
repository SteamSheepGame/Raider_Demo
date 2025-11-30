using UnityEngine;
using System;
namespace Demo.Core
{
    public class WorkTask
    {
        public float WorkHours;
        public string RewardResourceId;
        public int RewardAmount;
        public string WorkName;
        public WorkTimeConsumer WorkTimer;

        public WorkTask(string rewardId, string workName, int rewardAmount, float workHours)
        {
            RewardResourceId = rewardId;
            WorkName = workName;
            RewardAmount = rewardAmount;
            WorkHours = workHours;
        }
        
        public void StartTask()
        {
           WorkTimer = new WorkTimeConsumer();
           WorkTimer.Init(WorkName, WorkHours);
        }
            
    }
    public class WorkTimeConsumer: ITimeConsumer
    {
        public string Name { get; private set; }
        public float Lifetime { get; private set; }
        
        public event Action _OnFinished;

        public void Init(string name, float lifetime)
        {
            Name = name;
            Lifetime = lifetime;
            TimeSystem.Instance.RegisterTimeConsumer(this);
        }
        
        public void OnLifetimeExpired()
        {
            _OnFinished.Invoke();
            TimeSystem.Instance.UnregisterTimeConsumer(this);
        }
    }
}