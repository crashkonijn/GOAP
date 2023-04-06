using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using ICondition = CrashKonijn.Goap.Resolver.Interfaces.ICondition;
using IEffect = CrashKonijn.Goap.Resolver.Interfaces.IEffect;

namespace CrashKonijn.Goap.Behaviours
{

    public abstract class ActionBase<TActionData> : ActionBase
        where TActionData : IActionData, new()
    {
        public override IActionData GetData()
        {
            return this.CreateData();
        }

        public virtual TActionData CreateData()
        {
            return new TActionData();
        }

        public override void OnStart(IMonoAgent agent, IActionData data) => this.OnStart(agent, (TActionData) data);
        
        public abstract void OnStart(IMonoAgent agent, TActionData data);

        public override ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context) => this.Perform(agent, (TActionData) data, context);

        public abstract ActionRunState Perform(IMonoAgent agent, TActionData data, ActionContext context);

        public override void OnEnd(IMonoAgent agent, IActionData data) => this.OnEnd(agent, (TActionData) data);
        
        public abstract void OnEnd(IMonoAgent agent, TActionData data);
    }

    public abstract class ActionBase : IActionBase
    {
        private IActionConfig config;
        
        public IActionConfig Config => this.config;
        
        // IAction
        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects => this.config.Effects.Cast<IEffect>().ToArray();
        public ICondition[] Conditions => this.config.Conditions.Cast<ICondition>().ToArray();

        public void SetConfig(IActionConfig config)
        {
            this.config = config;
        }

        public virtual float GetDistanceCost(ITarget currentTarget, ITarget otherTarget)
        {
            if (currentTarget == null || otherTarget == null)
                return 0;
            
            return Vector3.Distance(currentTarget.Position, otherTarget.Position);
        }
        
        public virtual int GetCost(IWorldData data)
        {
            return this.config.BaseCost;
        }

        public abstract IActionData GetData();
        public abstract ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context);
        public abstract void OnStart(IMonoAgent agent, IActionData data);
        public abstract void OnEnd(IMonoAgent agent, IActionData data);
    }
}