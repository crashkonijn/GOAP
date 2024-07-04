using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public class NodeEffect : INodeEffect
    {
        public IEffect Effect { get; set; }
        public INode[] Connections { get; set; } = Array.Empty<INode>();
    }
}