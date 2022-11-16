using System.Collections.Generic;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Models
{
    internal class NodeCondition
    {
        public ICondition Condition { get; set; }
        public HashSet<Node> Connections { get; set; } = new HashSet<Node>();
    }
}