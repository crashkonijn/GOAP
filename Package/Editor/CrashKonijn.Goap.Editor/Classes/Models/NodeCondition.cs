using System;

namespace CrashKonijn.Goap.Editor
{
    public class NodeCondition
    {
        public string Condition { get; set; }
        
        public Guid[] Connections { get; set; } = {};
    }
}