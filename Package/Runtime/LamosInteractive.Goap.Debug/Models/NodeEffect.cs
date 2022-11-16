using System;
using System.Collections.Generic;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Debug.Models
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public HashSet<Guid> Connections { get; set; } = new HashSet<Guid>();
    }
}