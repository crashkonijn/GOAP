using System;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Models
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public Node[] Connections { get; set; } = Array.Empty<Node>();
    }
}