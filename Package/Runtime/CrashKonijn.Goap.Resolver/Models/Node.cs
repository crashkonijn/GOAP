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

        public void GetActions(List<IGoapAction> actions)
        {
            if (actions.Contains(this.Action as IGoapAction))
                return;

            if (this.Action is IGoapAction goapAction)
                actions.Add(goapAction);

            foreach (var condition in this.Conditions)
            {
                foreach (var connection in condition.Connections)
                {
                    connection.GetActions(actions);
                }
            }
        }
    }
}
