using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Resolver.Models
{
    public class Graph : IGraph
    {
        public List<INode> RootNodes { get; set; } = new();
        public List<INode> ChildNodes { get; set; } = new();
        public INode[] AllNodes => this.RootNodes.Union(this.ChildNodes).ToArray();
    }
}