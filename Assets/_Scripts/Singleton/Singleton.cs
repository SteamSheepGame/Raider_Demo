using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Demo.Core
{
    public class Singleton<T> : SerializedMonoBehaviour where T : Singleton<T>
    {
        #region 单例变量与属性
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                    if (_instance == null)
                    {
                        Debug.LogError($"[Singleton] 场景中找不到 {typeof(T).Name} 实例，请确保场景中存在或手动创建。");
                    }
                }
                return _instance;
            }
        }
        #endregion
        
        [OdinSerialize] private bool _isDontDestroyOnLoad = true;

        protected virtual void Initialize()
        {
            
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (_isDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                    Debug.Log($"[SingletonBase] {typeof(T).Name} 启用 DontDestroyOnLoad。");
                }
                
                Initialize();
            }
            else
            {
                if (_instance != this)
                {
                    Debug.LogWarning($"[Singleton] 检测到重复单例 {typeof(T).Name}，自动销毁后来的实例。");
                    Destroy(gameObject);
                }
            }
        }
    }
}