using System;
using System.Collections.Generic;
using LamosInteractive.Goap.Interfaces;
using LamosInteractive.Goap.Models;

namespace LamosInteractive.Goap.Debug.Models
{
    public class Node
    {
        public Guid Guid => Action.Guid;
        
        public IAction Action { get; set; }

        public HashSet<NodeEffect> Effects { get; set; } = new HashSet<NodeEffect>();
        public HashSet<NodeCondition> Conditions { get; set; } = new HashSet<NodeCondition>();
    }
}