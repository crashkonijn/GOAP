using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Classes.References
{
    public interface IComponentReference
    {
        T GetComponent<T>()
            where T : MonoBehaviour;
    }

    public interface IDataReferenceInjector : IComponentReference
    {
        void Inject(IActionData data);
    }

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
                var attributes = property.GetCustomAttributes(typeof(GetComponentAttribute), true);
                
                if (attributes.Length == 0)
                    continue;
                
                // set the reference
                property.SetValue(data, this.GetComponentReference(property.PropertyType));
            }
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
    }
}