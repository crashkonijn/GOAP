using System;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Models
{
    public class NodeCondition
    {
        public ICondition Condition { get; set; }
        public Node[] Connections { get; set; } = Array.Empty<Node>();
    }
}