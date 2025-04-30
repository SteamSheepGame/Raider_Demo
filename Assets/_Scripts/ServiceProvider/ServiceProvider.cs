using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Core
{
    public class ServiceProvider : Singleton<ServiceProvider>
    {
        //存放服务类实例的字典
        private Dictionary<Type, IService> _services;

        protected override void Initialize()
        {
            _services = new Dictionary<Type,IService>();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="service">服务类实例</param>
        /// <typeparam name="T">服务类接口</typeparam>
        public void RegisterService<T>(T service) where T : IService
        {
            Type key = typeof(T);
            // 检查服务类型是否为接口
            if (!key.IsInterface)
            {
                Debug.LogError($"[ServiceProvider] [Register] 非接口的注册键: {key.Name}");
                return;
            }
            
            // 检查服务实例是否为空
            if (service == null)
            {
                Debug.LogError($"[ServiceProvider] [Register] 空服务: {key.Name}");
                return;
            }
            
            // 检查服务是否已经注册
            if (Exists<T>())
            {
                Debug.LogWarning($"[ServiceProvider] [Register] 覆盖注册: {key.Name}");
            }
            
#if UNITY_EDITOR
            Debug.Log($"[ServiceProvider] [Register] 成功注册: {key.Name}");
#endif
            _services[key] = service;
        }
        
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">服务类接口</typeparam>
        /// <returns></returns>
        public T GetService<T>() where T : IService
        {
            Type key = typeof(T);
            // 检查服务类型是否为接口
            if (!key.IsInterface)
            {
                Debug.LogError($"[ServiceProvider] [GetService] 非接口: {key.Name}");
                return default;
            }
            
            // 检查服务是否存在于字典中
            if (!Exists<T>())
            {
                Debug.LogError($"[ServiceProvider] [GetService] 不存在: {key.Name}");
                return default;
            }

            IService service = _services[key];
            // 检查服务实例是否为空
            if (service == null)
            {
                Debug.LogError($"[ServiceProvider] [GetService] 空服务: {key.Name}");
                return default;
            }
            
            // 检查服务类型是否匹配
            if (!key.IsAssignableFrom(service.GetType()))
            {
                Debug.LogError($"[ServiceProvider] [GetService] 类型不匹配: {key.Name}");
                return default;
            }
            return (T)service;
        }
        
        /// <summary>
        /// 注销服务
        /// </summary>
        /// <typeparam name="T">服务类接口</typeparam>
        public void UnregisterService<T>() where T : IService
        {
            Type key = typeof(T);
            // 检查服务类型是否存在于字典中
            if (!Exists<T>())
            {
                Debug.LogWarning($"[ServiceProvider] [UnregisterService] 不存在: {key.Name}");
                return;
            }
            _services.Remove(key);
        }

        public void UnregisterAllServices()
        {
            _services.Clear();
            
#if UNITY_EDITOR
            Debug.Log($"[ServiceProvider] [UnregisterAllServices] 清除所有服务");
#endif
        }

        /// <summary>
        /// 判断服务是否存在（被注册）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool Exists<T>() where T : IService
        {
            return _services.ContainsKey(typeof(T));
        }
    }
}