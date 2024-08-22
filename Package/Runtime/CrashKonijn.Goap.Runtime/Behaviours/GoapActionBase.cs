using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    // Backwards compatability for v2 actions
    [Obsolete("Use GoapActionBase instead of ActionBase")]
    public abstract class ActionBase<TActionData> : GoapActionBase<TActionData, EmptyActionProperties>
        where TActionData : IActionData, new()
    {
        public override IActionRunState Perform(IMonoAgent agent, TActionData data, IActionContext context)
        {
            return this.Perform(agent, data, context as ActionContext);
        }

        public abstract ActionRunState Perform(IMonoAgent agent, TActionData data, ActionContext context);
    }
    
    public abstract class GoapActionBase<TActionData> : GoapActionBase<TActionData, EmptyActionProperties>
        where TActionData : IActionData, new()
    {
    }
    
    public abstract class GoapActionBase<TActionData, TActionProperties> : AgentActionBase<TActionData, TActionProperties>, IGoapAction
        where TActionData : IActionData, new()
        where TActionProperties : class, IActionProperties, new()
    {
        public IActionConfig Config { get; private set; }

        public Guid Guid { get; } = Guid.NewGuid();
        public IEffect[] Effects => this.Config.Effects;
        public ICondition[] Conditions => this.Config.Conditions;
        
        public override TActionProperties Properties => this.Config.Properties as TActionProperties;

        public void SetConfig(IActionConfig config)
        {
            this.Config = config;
        }
        
        public virtual float GetCost(IActionReceiver agent, IComponentReference references)
        {
            return this.Config.BaseCost;
        }

        public ActionMoveMode GetMoveMode(IMonoAgent agent)
        {
            return this.Config.MoveMode;
        }

        public virtual float GetStoppingDistance()
        {
            return this.Config.StoppingDistance;
        }

        public virtual bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references)
        {
            return distance <= this.GetStoppingDistance();
        }

        public bool IsValid(IActionReceiver agent, IActionData data)
        {
            var goapAgent = agent.Injector.GetCachedComponent<GoapActionProvider>();
            
            if (goapAgent == null)
                return false;
            
            if (this.Config.ValidateConditions && !goapAgent.AgentType.AllConditionsMet(goapAgent, this))
            {
                agent.Logger.Warning($"Conditions not met: {this.Config.Name}");
                return false;
            }

            if (this.Config.RequiresTarget && goapAgent.WorldData.GetTarget(this) == null)
            {
                agent.Logger.Warning($"No target found for: {this.Config.Name}");
                return false;
            }

            return this.IsValid(agent, (TActionData) data);
        }
        
        public virtual bool IsValid(IActionReceiver agent, TActionData data)
        {
            return true;
        }
        
        public bool IsExecutable(IActionReceiver agent, bool conditionsMet)
        {
            var goapAgent = agent.Injector.GetCachedComponent<GoapActionProvider>();
            
            if (goapAgent == null)
                return false;
            
            if (!conditionsMet)
                return false;
            
            if (this.Config.RequiresTarget && goapAgent.WorldData.GetTarget(this) == null)
                return false;

            return true;
        }
    }
}