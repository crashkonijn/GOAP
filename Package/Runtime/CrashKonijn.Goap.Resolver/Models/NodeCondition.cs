using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public class NodeCondition : INodeCondition
    {
        public ICondition Condition { get; set; }
        public INode[] Connections { get; set; } = Array.Empty<INode>();
    }
}