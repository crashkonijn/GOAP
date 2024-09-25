using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class GoalBase : IGoal
    {
        public int Index { get; set; }
        private IGoalConfig config;
        public IGoalConfig Config => this.config;
        
        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects { get; } = {};
        
        public ICondition[] Conditions { get; private set; }

        public void SetConfig(IGoalConfig config)
        {
            this.config = config;
            this.Conditions = config.Conditions.ToArray();
        }

        public virtual float GetCost(IActionReceiver agent, IComponentReference references)
        {
            return this.config.BaseCost;
        }
    }
}