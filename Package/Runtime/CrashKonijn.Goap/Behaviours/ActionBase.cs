using System;
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
        
        public TActionProperties Properties => this.Config.Properties as TActionProperties;

        public override void Start(IMonoAgent agent, IActionData data) => this.Start(agent, (TActionData) data);
        
        public abstract void Start(IMonoAgent agent, TActionData data);
        public override bool IsValid(IMonoAgent agent, IActionData data)
        {
            if (!agent.AgentType.AllConditionsMet(agent, this))
            {
                agent.Logger.Warning($"Conditions not met for {agent.name}: {this.Config.Name}");
                return false;
            }

            if (this.Config.RequiresTarget && agent.WorldData.GetTarget(this) == null)
            {
                agent.Logger.Warning($"No target found for {agent.name}: {this.Config.Name}");
                return false;
            }

            return this.IsValid(agent, (TActionData) data);
        }

        public virtual bool IsValid(IMonoAgent agent, TActionData data)
        {
            return true;
        }

        public override bool IsExecutable(IMonoAgent agent, bool conditionsMet)
        {
            if (!conditionsMet)
                return false;
            
            if (this.Config.RequiresTarget && agent.WorldData.GetTarget(this) == null)
                return false;

            return true;
        }
        
        public override bool IsEnabled(IMonoAgent agent)
        {
            return this.IsEnabled(agent, agent.Injector);
        }
        
        public virtual bool IsEnabled(IMonoAgent agent, IComponentReference references)
        {
            return !agent.DisabledActions.Contains(this.GetType());
        }
        
        public override void BeforePerform(IMonoAgent agent, IActionData data) => this.BeforePerform(agent, (TActionData) data);
        
        public virtual void BeforePerform(IMonoAgent agent, TActionData data) {}

        public override IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context) => this.Perform(agent, (TActionData) data, context);

        public abstract IActionRunState Perform(IMonoAgent agent, TActionData data, IActionContext context);
        public virtual void End(IMonoAgent agent, TActionData data) {}

        public override void Stop(IMonoAgent agent, IActionData data)
        {
            this.Stop(agent, (TActionData)data);
            this.End(agent, (TActionData)data);
        }
        
        public virtual void Stop(IMonoAgent agent, TActionData data) {}

        public override void Complete(IMonoAgent agent, IActionData data)
        {
            this.Complete(agent, (TActionData) data);
            this.End(agent, (TActionData) data);
        }
        
        public virtual void Complete(IMonoAgent agent, TActionData data) {}
    }

    public abstract class ActionBase : IAction
    {
        public IActionConfig Config { get; private set; }

        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects => this.Config.Effects;
        public ICondition[] Conditions => this.Config.Conditions;

        public void SetConfig(IActionConfig config)
        {
            this.Config = config;
        }
        
        public virtual float GetCost(IMonoAgent agent, IComponentReference references)
        {
            return this.Config.BaseCost;
        }
        
        [Obsolete("Please use the IsInRange method")]
        public virtual float GetInRange(IMonoAgent agent, IActionData data)
        {
            return this.Config.StoppingDistance;
        }
        
        public virtual bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references)
        {
#pragma warning disable CS0618
            return distance <= this.GetInRange(agent, data);
#pragma warning restore CS0618
        }

        public abstract IActionData GetData();
        public abstract void Created();
        public abstract bool IsValid(IMonoAgent agent, IActionData data);

        public abstract void Start(IMonoAgent agent, IActionData data);
        public abstract void BeforePerform(IMonoAgent agent, IActionData data);
        public abstract IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context);
        public abstract void Stop(IMonoAgent agent, IActionData data);
        public abstract void Complete(IMonoAgent agent, IActionData data);
        public abstract bool IsExecutable(IMonoAgent agent, bool conditionsMet);
        public abstract bool IsEnabled(IMonoAgent agent);
    }
}