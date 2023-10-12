using System;

namespace CrashKonijn.Goap.Attributes.Ids
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class WorldKeyIdAttribute : Attribute
    {
        public readonly string Id;
        
        public WorldKeyIdAttribute(string id)
        {
            this.Id = id;
        }
    }
}