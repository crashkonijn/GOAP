using System;
using System.Collections.Generic;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Debug.Models
{
    public class NodeCondition
    {
        public string Condition { get; set; }
        
        public HashSet<Guid> Connections { get; set; } = new HashSet<Guid>();
    }
}