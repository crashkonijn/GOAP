using System;
using System.Collections.Generic;
using System.Linq;

namespace LamosInteractive.Goap.Debug.Models
{
    public class Graph
    {
        public Dictionary<Guid, Node> RootNodes { get; set; } = new Dictionary<Guid, Node>();
        public Dictionary<Guid, Node> ChildNodes { get; set; } = new Dictionary<Guid, Node>();
        public Dictionary<Guid, Node> AllNodes => RootNodes.Union(ChildNodes).ToDictionary(x => x.Key, x => x.Value);
    }
}