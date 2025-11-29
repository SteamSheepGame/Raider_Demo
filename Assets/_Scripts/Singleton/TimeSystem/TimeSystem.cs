using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Demo.Core
{
    public interface ITimeConsumer
    {
        float Lifetime { get; }
        string Name { get; }
        void OnLifetimeExpired();
    }
    
    [Serializable]
    public struct GameTime
    {
        public int day;
        public int hour;
        private float hourAccumulator;
        /// <summary>
        /// 更新游戏时间【第几天+小时】
        /// </summary>
        /// <param name="deltaTime"></param>
        public void AdvanceTime(float deltaTime)
        {
            hourAccumulator += deltaTime / 60f;   // accumulate fractional hours

            while (hourAccumulator >= 1f)
            {
                hour++;
                hourAccumulator -= 1f;

                if (hour >= 24)
                {
                    hour = 0;
                    day++;
                }
            }
        }

        public override string ToString()
        {
            return $"Day {day}, {hour:00}:00";
        }
    }
    
    
    public class TimeSystem : Singleton<TimeSystem>
    {
        private float _timeScale = 1f;
        private bool _isPaused = true;
        private GameTime _gameTime;
        
        private readonly PriorityQueue<ITimeConsumer, float> _consumers = new PriorityQueue<ITimeConsumer, float>();
        private readonly Dictionary<ITimeConsumer, float> _remainingTimes = new Dictionary<ITimeConsumer, float>();
        
        private const float UPDATE_INTERVAL = 0.1f;
        
        #region Debug
        [ShowInInspector, TableList(ShowIndexLabels = true, AlwaysExpanded = true)]
        private List<ConsumerDebugInfo> DebugConsumers => GetDebugConsumers();
        
        [ShowInInspector, ReadOnly]
        private GameTime CurrentGameTime => _gameTime;
        
        [Serializable]
        private class ConsumerDebugInfo
        {
            [ReadOnly] public string name;
            [ReadOnly, DisplayAsString(Format = "0.000")] public float remainingTime; // 毫秒精度
            [ReadOnly] public float originalLifetime;
        }
        
        private List<ConsumerDebugInfo> GetDebugConsumers()
        {
            var debugList = new List<ConsumerDebugInfo>();
            foreach (var (consumer, _) in _consumers.GetAll())
            {
                if (_remainingTimes.TryGetValue(consumer, out float remainingTime))
                {
                    debugList.Add(new ConsumerDebugInfo
                    {
                        name = consumer.Name,
                        remainingTime = remainingTime,
                        originalLifetime = consumer.Lifetime
                    });
                }
            }
            return debugList;
        }
        #endregion

        private void Start()
        {
            _gameTime = new GameTime { day = 1, hour = 0 };
            
            Resume(); //默认立即开始时间系统
            StartCoroutine(UpdateConsumersRoutine());
        }

        public void RegisterTimeConsumer(ITimeConsumer consumer)
        {
            if (consumer == null || consumer.Lifetime <= 0) return;
            _consumers.Enqueue(consumer, consumer.Lifetime);
            _remainingTimes[consumer] = consumer.Lifetime;
        }
        
        public void UnregisterTimeConsumer(ITimeConsumer consumer)
        {
            if (consumer == null) return;
            _consumers.Remove(consumer);
            _remainingTimes.Remove(consumer);
        }
        
        [Button]
        public void Pause() => _isPaused = true;
        [Button]
        public void Resume() => _isPaused = false;
        [Button]
        public void SetTimeScale(float timeScale) => _timeScale = Mathf.Max(0, timeScale);
        
        public float GetTimeScale() => _timeScale;
        public bool IsPaused() => _isPaused;
        public GameTime GetGameTime() => _gameTime;

        private IEnumerator UpdateConsumersRoutine()
        {
            while (true)
            {
                if (!_isPaused && _consumers.Count > 0)
                {
                    float deltaTime = UPDATE_INTERVAL * _timeScale;
                    _gameTime.AdvanceTime(deltaTime); // 更新游戏时间
                    
                    var expiredConsumers = new List<ITimeConsumer>(); //用于集中处理的过期对象
                    var consumersCopy = new List<(ITimeConsumer consumer, float priority)>(_consumers.GetAll());
                    
                    // 更新每个消耗者的剩余时间
                    foreach (var (consumer, _) in consumersCopy)
                    {
                        if (!_remainingTimes.TryGetValue(consumer, out float remainingTime)) continue;
                        remainingTime = Mathf.Max(0, remainingTime - deltaTime);
                        _remainingTimes[consumer] = remainingTime;
                        
                        _consumers.UpdatePriority(consumer, remainingTime);
                        if (remainingTime <= 0)
                        {
                            expiredConsumers.Add(consumer);
                        }
                    }
                    
                    foreach (var consumer in expiredConsumers)
                    {
                        _consumers.Remove(consumer);
                        _remainingTimes.Remove(consumer);
                        consumer.OnLifetimeExpired();
                    }
                }
                yield return new WaitForSeconds(UPDATE_INTERVAL);
            }
        }
    }
}