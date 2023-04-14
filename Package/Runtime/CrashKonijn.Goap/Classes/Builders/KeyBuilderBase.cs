using System;
using System.Collections.Generic;

namespace CrashKonijn.Goap.Classes.Builders
{
    public abstract class KeyBuilderBase<TKey, TInterface>
        where TKey : TInterface
    {
        private Dictionary<string, TKey> keys = new();
        
        public TInterface GetKey(string name)
        {
            if (this.keys.TryGetValue(name, out var key))
            {
                return key;
            }

            key = (TKey) Activator.CreateInstance(typeof(TKey), name);
            this.keys.Add(name, key);
            
            return key;
        }
        
        public TInterface GetKey<T>(string name)
        {
            var dynamicName = $"{name}<{typeof(T).Name}>";
            
            if (this.keys.TryGetValue(dynamicName, out var key))
            {
                return key;
            }

            key = (TKey) Activator.CreateInstance(typeof(TKey), dynamicName);
            this.keys.Add(dynamicName, key);
            
            return key;
        }
        
        public TInterface GetKey<T1, T2>(string name)
        {
            var dynamicName = $"{name}<{typeof(T1).Name},{typeof(T2).Name}>";
            
            if (this.keys.TryGetValue(dynamicName, out var key))
            {
                return key;
            }

            key = (TKey) Activator.CreateInstance(typeof(TKey), dynamicName);
            this.keys.Add(dynamicName, key);
            
            return key;
        }
    }
}