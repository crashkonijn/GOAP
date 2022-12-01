using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class GoalBase : IGoalBase
    {
        private IGoalConfig config;
        
        public IGoalConfig Config => this.config;
        
        // IAction
        public Guid Guid { get; } = Guid.NewGuid();
        public HashSet<IEffect> Effects { get; } = new();
        public HashSet<ICondition> Conditions => this.config.Conditions.Cast<ICondition>().ToHashSet();

        public void SetConfig(IGoalConfig config)
        {
            this.config = config;
        }

        public virtual int GetCost(IWorldData data)
        {
            return this.config.BaseCost;
        }
    }
}