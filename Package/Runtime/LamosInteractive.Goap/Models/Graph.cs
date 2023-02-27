using System;
using System.Collections.Generic;
using System.Linq;

namespace LamosInteractive.Goap.Models
{
    public class Graph
    {
        public List<Node> RootNodes { get; set; } = new();
        public List<Node> ChildNodes { get; set; } = new();
        public Node[] AllNodes => RootNodes.Union(ChildNodes).ToArray();
    }
}