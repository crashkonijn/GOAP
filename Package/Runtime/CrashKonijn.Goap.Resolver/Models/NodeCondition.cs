using System;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Resolver.Models
{
    public class NodeCondition : INodeCondition
    {
        public ICondition Condition { get; set; }
        public INode[] Connections { get; set; } = Array.Empty<INode>();
    }
}