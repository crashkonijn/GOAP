using System;
using System.Collections.Generic;
using System.Reflection;
using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public class DataReferenceInjector : IDataReferenceInjector
    {
        private readonly IMonoAgent agent;
        private readonly Dictionary<Type, object> references = new();
        private readonly Dictionary<PropertyInfo, object> cachedReferences = new();

        private static readonly Dictionary<Type, DataReferenceCache> CachedDataReferenceCaches = new();

        public DataReferenceInjector(IMonoAgent agent)
        {
            this.agent = agent;
        }

        public void Inject(IActionData data)
        {
            var reference = this.GetCachedDataReferenceCache(data.GetType());

            foreach (var (propertyInfo, attribute) in reference.Properties)
            {
                var value = this.GetCachedPropertyValue(propertyInfo, attribute);

                if (value == null)
                    continue;

                // set the reference
                propertyInfo.SetValue(data, value);
            }
        }

        private DataReferenceCache GetCachedDataReferenceCache(Type type)
        {
            if (!CachedDataReferenceCaches.ContainsKey(type))
                CachedDataReferenceCaches.Add(type, new DataReferenceCache(type));

            return CachedDataReferenceCaches[type];
        }

        private object GetPropertyValue(PropertyInfo property, Attribute attribute)
        {
            if (attribute is GetComponentAttribute)
                return this.GetCachedComponentReference(property.PropertyType);

            if (attribute is GetComponentInChildrenAttribute)
                return this.GetCachedComponentInChildrenReference(property.PropertyType);

            if (attribute is GetComponentInParentAttribute)
                return this.GetCachedComponentInParentReference(property.PropertyType);

            return null;
        }

        private object GetCachedPropertyValue(PropertyInfo property, Attribute attribute)
        {
            if (!this.cachedReferences.ContainsKey(property))
            {
                this.cachedReferences.Add(property, this.GetPropertyValue(property, attribute));
            }

            return this.cachedReferences[property];
        }

        private object GetCachedComponentReference(Type type)
        {
            // check if we have a reference for this type
            if (!this.references.ContainsKey(type))
                this.references.Add(type, this.agent.GetComponent(type));

            // get the reference
            return this.references[type];
        }

        [Obsolete("'GetComponent<T>' is deprecated, please use 'GetCachedComponent<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        public T GetComponent<T>()
        {
            return (T) this.GetCachedComponentReference(typeof(T));
        }

        public T GetCachedComponent<T>()
        {
            return (T) this.GetCachedComponentReference(typeof(T));
        }

        private object GetCachedComponentInChildrenReference(Type type)
        {
            // check if we have a reference for this type
            if (!this.references.ContainsKey(type))
                this.references.Add(type, this.agent.GetComponentInChildren(type));

            // get the reference
            return this.references[type];
        }

        [Obsolete("'GetComponentInChildren<T>' is deprecated, please use 'GetCachedComponentInChildren<T>' instead.   Exact same functionality, name changed to better communicate code usage.")]
        public T GetComponentInChildren<T>()
        {
            return (T) this.GetCachedComponentInChildrenReference(typeof(T));
        }

        public T GetCachedComponentInChildren<T>()
        {
            return (T) this.GetCachedComponentInChildrenReference(typeof(T));
        }

        private object GetCachedComponentInParentReference(Type type)
        {
            // check if we have a reference for this type
            if (!this.references.ContainsKey(type))
                this.references.Add(type, this.agent.GetComponentInParent(type));

            // get the reference
            return this.references[type];
        }

        public T GetCachedComponentInParent<T>()
        {
            return (T) this.GetCachedComponentInParentReference(typeof(T));
        }

        private class DataReferenceCache
        {
            public Dictionary<PropertyInfo, Attribute> Properties { get; private set; }

            public DataReferenceCache(Type type)
            {
                this.Properties = new Dictionary<PropertyInfo, Attribute>();

                // find all properties with the GetComponent attribute
                var props = type.GetProperties();

                foreach (var prop in props)
                {
                    foreach (var attribute in prop.GetCustomAttributes(true))
                    {
                        if (attribute is not (GetComponentAttribute or GetComponentInChildrenAttribute or GetComponentInParentAttribute))
                            continue;

                        this.Properties.Add(prop, (Attribute) attribute);
                        break;
                    }
                }
            }
        }
    }
}
