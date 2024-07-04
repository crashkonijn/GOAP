using System;
using System.Collections.Generic;
using System.Linq;

namespace CrashKonijn.Goap.Editor
{
    public class Graph
    {
        public Dictionary<Guid, Node> RootNodes { get; set; } = new Dictionary<Guid, Node>();
        public Dictionary<Guid, Node> ChildNodes { get; set; } = new Dictionary<Guid, Node>();
        public Dictionary<Guid, Node> AllNodes => this.RootNodes.Union(this.ChildNodes).ToDictionary(x => x.Key, x => x.Value);
    }
}