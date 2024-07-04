using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public class Node : INode
    {
        public Guid Guid { get; } = Guid.NewGuid();

        public IConnectable Action { get; set; }

        public List<INodeEffect> Effects { get; set; } = new();
        public List<INodeCondition> Conditions { get; set; } = new();

        public bool IsRootNode => this.Action is IGoal;
    }
}