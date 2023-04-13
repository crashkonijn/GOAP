using System;
using System.Collections.Generic;

namespace CrashKonijn.Goap.Editor.Classes.Models
{
    public class NodeCondition
    {
        public string Condition { get; set; }
        
        public Guid[] Connections { get; set; } = {};
    }
}