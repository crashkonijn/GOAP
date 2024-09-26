using System;

namespace CrashKonijn.Goap.Runtime
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class GoapIdAttribute : Attribute
    {
        public readonly string Id;

        public GoapIdAttribute(string id)
        {
            this.Id = id;
        }
    }
}
