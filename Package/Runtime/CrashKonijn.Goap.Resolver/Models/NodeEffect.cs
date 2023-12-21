using System;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Resolver.Models
{
    public class NodeEffect : INodeEffect
    {
        public IEffect Effect { get; set; }
        public INode[] Connections { get; set; } = Array.Empty<INode>();
    }
}