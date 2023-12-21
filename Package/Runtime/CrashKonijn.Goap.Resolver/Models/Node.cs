using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Resolver.Models
{
    public class Node : INode
    {
        public Guid Guid { get; } = Guid.NewGuid();
        
        public IConnectable Action { get; set; }

        public List<INodeEffect> Effects { get; set; } = new ();
        public List<INodeCondition> Conditions { get; set; } = new ();

        public bool IsRootNode => this.Action.Effects == null || !this.Action.Effects.Any();
    }
}
