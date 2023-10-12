using System;

namespace CrashKonijn.Goap.Attributes.Ids
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class SensorIdAttribute : Attribute
    {
        public readonly string Id;
        
        public SensorIdAttribute(string id)
        {
            this.Id = id;
        }
    }
}