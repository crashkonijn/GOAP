using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes.References
{
    public class DataReferenceInjector : IDataReferenceInjector
    {
        private readonly IMonoAgent agent;
        private readonly Dictionary<Type, object> references = new();

        public DataReferenceInjector(IMonoAgent agent)
        {
            this.agent = agent;
        }
        
        public void Inject(IActionData data)
        {
            var type = data.GetType();
            
            // find all properties with the GetComponent attribute
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var value = this.GetPropertyValue(property);
                
                if (value == null)
                    continue;

                // set the reference
                property.SetValue(data, value);
            }
        }

        private object GetPropertyValue(PropertyInfo property)
        {
            if (property.GetCustomAttributes(typeof(GetComponentAttribute), true).Any())
                return this.GetCachedComponentReference(property.PropertyType);
            
            if (property.GetCustomAttributes(typeof(GetComponentInChildrenAttribute), true).Any())
                return this.GetCachedComponentInChildrenReference(property.PropertyType);
            
            return null;
        }

        private object GetCachedComponentReference(Type type)
        {
            // check if we have a reference for this type
            if (!this.references.ContainsKey(type))
                this.references.Add(type, this.agent.GetComponent(type));
                
            // get the reference
            return this.references[type];
        }

        [System.Obsolete("'GetComponent<T>' is deprecated, please use 'GetCachedComponent<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        public T GetComponent<T>()
            where T : MonoBehaviour
        {
            return (T) this.GetCachedComponentReference(typeof(T));
        }
        
        public T GetCachedComponent<T>()
            where T : MonoBehaviour
        {
            return (T)this.GetCachedComponentReference(typeof(T));
        }

        private object GetCachedComponentInChildrenReference(Type type)
        {
            // check if we have a reference for this type
            if (!this.references.ContainsKey(type))
                this.references.Add(type, this.agent.GetComponentInChildren(type));
                
            // get the reference
            return this.references[type];
        }

        [System.Obsolete("'GetComponentInChildren<T>' is deprecated, please use 'GetCachedComponentInChildren<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        public T GetComponentInChildren<T>()
            where T : MonoBehaviour
        {
            return (T) this.GetCachedComponentInChildrenReference(typeof(T));
        }
        public T GetCachedComponentInChildren<T>()
            where T : MonoBehaviour
        {
            return (T)this.GetCachedComponentInChildrenReference(typeof(T));
        }
    }
}