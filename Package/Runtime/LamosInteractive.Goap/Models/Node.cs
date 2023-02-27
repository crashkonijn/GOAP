using System;
using System.Collections.Generic;
using System.Linq;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Models
{
    public class Node
    {
        public Guid Guid { get; } = Guid.NewGuid();
        
        public IAction Action { get; set; }

        public List<NodeEffect> Effects { get; set; } = new List<NodeEffect>();
        public List<NodeCondition> Conditions { get; set; } = new List<NodeCondition>();

        public bool IsRootNode => Action.Effects == null || !Action.Effects.Any();
    }
}
