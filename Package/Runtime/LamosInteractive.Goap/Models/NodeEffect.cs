using System.Collections.Generic;
using LamosInteractive.Goap.Interfaces;

namespace LamosInteractive.Goap.Models
{
    internal class NodeEffect
    {
        public IEffect Effect { get; set; }
        public HashSet<Node> Connections { get; set; } = new HashSet<Node>();
    }
}