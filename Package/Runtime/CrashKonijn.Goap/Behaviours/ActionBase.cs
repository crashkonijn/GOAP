using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
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

        public override void Start(IMonoAgent agent, IActionData data) => this.Start(agent, (TActionData) data);
        
        public abstract void Start(IMonoAgent agent, TActionData data);

        public override ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context) => this.Perform(agent, (TActionData) data, context);

        public abstract ActionRunState Perform(IMonoAgent agent, TActionData data, ActionContext context);

        public override void End(IMonoAgent agent, IActionData data) => this.End(agent, (TActionData) data);
        
        public abstract void End(IMonoAgent agent, TActionData data);
    }

    public abstract class ActionBase : IActionBase
    {
        private IActionConfig config;
        
        public IActionConfig Config => this.config;
        
        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects => this.config.Effects.Cast<IEffect>().ToArray();
        public ICondition[] Conditions => this.config.Conditions.Cast<ICondition>().ToArray();

        public void SetConfig(IActionConfig config)
        {
            this.config = config;
        }

        public virtual float GetCost(IMonoAgent agent, IComponentReference references)
        {
            return this.config.BaseCost;
        }
        
        [Obsolete("Please use the IsInRange method")]
        public virtual float GetInRange(IMonoAgent agent, IActionData data)
        {
            return this.config.InRange;
        }
        
        public virtual bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references)
        {
#pragma warning disable CS0618
            return distance <= this.GetInRange(agent, data);
#pragma warning restore CS0618
        }

        public abstract IActionData GetData();
        public abstract void Created();
        public abstract ActionRunState Perform(IMonoAgent agent, IActionData data, ActionContext context);
        public abstract void Start(IMonoAgent agent, IActionData data);
        public abstract void End(IMonoAgent agent, IActionData data);
    }
}