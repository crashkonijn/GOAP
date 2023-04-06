using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.References
{
    public class DataReferenceInjector
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
                
                // get the type of the property
                var propertyType = property.PropertyType;
                
                // check if we have a reference for this type
                if (!this.references.ContainsKey(propertyType))
                    this.GetComponentReference(propertyType);
                
                // get the reference
                var reference = this.references[propertyType];
                
                // set the reference
                property.SetValue(data, reference);
            }
        }

        private void GetComponentReference(Type type)
        {
            this.references.Add(type, this.agent.GetComponent(type));
        }
    }
}