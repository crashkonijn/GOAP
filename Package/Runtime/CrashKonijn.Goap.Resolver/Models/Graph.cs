using System.Collections.Generic;
using System.Linq;

namespace CrashKonijn.Goap.Resolver.Models
{
    public class Graph
    {
        public List<Node> RootNodes { get; set; } = new();
        public List<Node> ChildNodes { get; set; } = new();
        public Node[] AllNodes => this.RootNodes.Union(this.ChildNodes).ToArray();
    }
}