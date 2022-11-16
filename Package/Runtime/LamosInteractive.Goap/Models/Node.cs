using System;
using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Models
{
    internal class Node
    {
        public Guid Guid { get; } = Guid.NewGuid();
        
        public IAction Action { get; set; }

        public HashSet<NodeEffect> Effects { get; set; } = new HashSet<NodeEffect>();
        public HashSet<NodeCondition> Conditions { get; set; } = new HashSet<NodeCondition>();

        public bool IsRootNode => Action.Effects == null || !Action.Effects.Any();
    }
}
