using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class GoalBase : IGoalBase
    {
        private GoalConfig config;
        
        public GoalConfig Config => this.config;
        
        // IAction
        public Guid Guid { get; } = Guid.NewGuid();
        public HashSet<IEffect> Effects { get; } = new();
        public HashSet<ICondition> Conditions => this.config.conditions.Cast<ICondition>().ToHashSet();

        public void SetConfig(GoalConfig config)
        {
            this.config = config;
        }

        public virtual int GetCost(IWorldData data)
        {
            return this.config.baseCost;
        }
    }
}