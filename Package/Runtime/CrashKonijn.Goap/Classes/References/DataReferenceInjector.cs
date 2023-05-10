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
                return this.GetComponentReference(property.PropertyType);
            
            if (property.GetCustomAttributes(typeof(GetComponentInChildrenAttribute), true).Any())
                return this.GetComponentInChildrenReference(property.PropertyType);
            
            return null;
        }

        private object GetComponentReference(Type type)
        {
            // check if we have a reference for this type
            if (!this.references.ContainsKey(type))
                this.references.Add(type, this.agent.GetComponent(type));
                
            // get the reference
            return this.references[type];
        }

        public T GetComponent<T>()
            where T : MonoBehaviour
        {
            return (T) this.GetComponentReference(typeof(T));
        }

        private object GetComponentInChildrenReference(Type type)
        {
            // check if we have a reference for this type
            if (!this.references.ContainsKey(type))
                this.references.Add(type, this.agent.GetComponentInChildren(type));
                
            // get the reference
            return this.references[type];
        }

        public T GetComponentInChildren<T>()
            where T : MonoBehaviour
        {
            return (T) this.GetComponentInChildrenReference(typeof(T));
        }
    }
}