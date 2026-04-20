using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Services = new();

        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);
            
            if (Services.ContainsKey(type))
            {
                Debug.LogWarning($"ServiceLocator: {type.Name} já foi registrado!");
                return;
            }

            Services[type] = service;
            Debug.Log($"ServiceLocator: {type.Name} registrado");
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (Services.TryGetValue(type, out var service))
            {
                return service as T;
            }

            Debug.LogError($"ServiceLocator: {type.Name} NÃO encontrado! Did you forget to Register?");
            return null;
        }

        public static bool TryGet<T>(out T service) where T : class
        {
            service = Get<T>();
            return service != null;
        }

        public static void Unregister<T>() where T : class
        {
            Services.Remove(typeof(T));
        }

        public static void ClearAll()
        {
            Services.Clear();
        }

        // Debug
        public static void DebugPrintRegistered()
        {
            var msg = "=== ServiceLocator Registered ===\n";
            foreach (var kvp in Services)
            {
                msg += $"- {kvp.Key.Name}: {kvp.Value.GetType().Name}\n";
            }
            Debug.Log(msg);
        }
    }
}