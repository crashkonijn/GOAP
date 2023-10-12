using System;

namespace CrashKonijn.Goap.Attributes.Ids
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ActionIdAttribute : Attribute
    {
        public readonly string Id;
        
        public ActionIdAttribute(string id)
        {
            this.Id = id;
        }
    }
}