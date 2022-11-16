using System.Collections.Generic;
using System.Linq;

namespace LamosInteractive.Goap.Models
{
    internal class Graph
    {
        public HashSet<Node> RootNodes { get; set; } = new HashSet<Node>();
        public HashSet<Node> ChildNodes { get; set; } = new HashSet<Node>();
        public HashSet<Node> AllNodes => RootNodes.Union(ChildNodes).ToHashSet();
    }
}