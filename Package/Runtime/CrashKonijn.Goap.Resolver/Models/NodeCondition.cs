using System;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Resolver.Models
{
    public class NodeCondition
    {
        public ICondition Condition { get; set; }
        public Node[] Connections { get; set; } = Array.Empty<Node>();
    }
}