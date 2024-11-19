using System;
using System.Collections.Generic;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class KeyBuilderBase<TInterface>
    {
        private Dictionary<Type, TInterface> keys = new();

        public TInterface GetKey<TKey>()
            where TKey : TInterface
        {
            return this.GetKey(typeof(TKey));
        }

        public TInterface GetKey(Type type)
        {
            if (this.keys.TryGetValue(type, out var key))
            {
                return key;
            }

            key = (TInterface) Activator.CreateInstance(type);

            this.keys.Add(type, key);

            return key;
        }
    }
}
