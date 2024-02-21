using System;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class EmptyActionProperties : IActionProperties
    {
    }
    
    // Backwards compatibility for old actions
    public abstract class ActionBase<TActionData> : ActionBase<TActionData, EmptyActionProperties>
        where TActionData : IActionData, new()
    {
        public ActionBase()
        {
            this.SetProperties(new EmptyActionProperties());
        }
    }

    public abstract class ActionBase<TActionData, TActionProperties> : ActionBase
        where TActionData : IActionData, new()
        where TActionProperties : class, IActionProperties, new()
    {
        public override IActionData GetData()
        {
            return this.CreateData();
        }

        public virtual TActionData CreateData()
        {
            return new TActionData();
        }
        
        public TActionProperties GetProperties()
        {
            return this.Properties as TActionProperties;
        }
        
        public T GetProperty<T>(Func<TActionProperties, T> func)
        {
            return func(this.GetProperties());
        }

        public override void Start(IMonoAgent agent, IActionData data) => this.Start(agent, (TActionData) data);
        
        public abstract void Start(IMonoAgent agent, TActionData data);

        public override IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context) => this.Perform(agent, (TActionData) data, context);

        public abstract IActionRunState Perform(IMonoAgent agent, TActionData data, IActionContext context);

        public override void Stop(IMonoAgent agent, IActionData data) => this.Stop(agent, (TActionData) data);
        
        public abstract void Stop(IMonoAgent agent, TActionData data);

        public override void Complete(IMonoAgent agent, IActionData data) => this.Complete(agent, (TActionData) data);
        
        public abstract void Complete(IMonoAgent agent, TActionData data);
    }

    public abstract class ActionBase : IAction
    {
        public IActionConfig Config { get; private set; }
        public IActionProperties Properties { get; private set; }

        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects => this.Config.Effects;
        public ICondition[] Conditions => this.Config.Conditions;

        public void SetConfig(IActionConfig config)
        {
            this.Config = config;
        }
        
        public void SetProperties(IActionProperties properties)
        {
            this.Properties = properties;
        }

        public virtual float GetCost(IMonoAgent agent, IComponentReference references)
        {
            return this.Config.BaseCost;
        }
        
        [Obsolete("Please use the IsInRange method")]
        public virtual float GetInRange(IMonoAgent agent, IActionData data)
        {
            return this.Config.InRange;
        }
        
        public virtual bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references)
        {
#pragma warning disable CS0618
            return distance <= this.GetInRange(agent, data);
#pragma warning restore CS0618
        }

        public abstract IActionData GetData();
        public abstract void Created();
        public abstract IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context);
        public abstract void Start(IMonoAgent agent, IActionData data);
        public abstract void Stop(IMonoAgent agent, IActionData data);
        public abstract void Complete(IMonoAgent agent, IActionData data);
    }
}