using System;

namespace CrashKonijn.Goap.Attributes.Ids
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TargetKeyIdAttribute : Attribute
    {
        public readonly string Id;
        
        public TargetKeyIdAttribute(string id)
        {
            this.Id = id;
        }
    }
}