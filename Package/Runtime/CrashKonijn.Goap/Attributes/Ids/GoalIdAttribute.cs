using System;

namespace CrashKonijn.Goap.Attributes.Ids
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class GoalIdAttribute : Attribute
    {
        public readonly string Id;
        
        public GoalIdAttribute(string id)
        {
            this.Id = id;
        }
    }
}