using System;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Resolver.Models
{
    public class NodeEffect
    {
        public IEffect Effect { get; set; }
        public Node[] Connections { get; set; } = Array.Empty<Node>();
    }
}