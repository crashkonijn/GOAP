using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    // Backwards compatability for v2 actions

    public abstract class GoapActionBase<TActionData> : GoapActionBase<TActionData, EmptyActionProperties>
        where TActionData : IActionData, new() { }

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

        [Obsolete("Use GetCost(IActionReceiver agent, IComponentReference references, ITarget target) instead")]
        public virtual float GetCost(IActionReceiver agent, IComponentReference references)
        {
            return this.Config.BaseCost;
        }

        public virtual float GetCost(IActionReceiver agent, IComponentReference references, ITarget target)
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
            if (agent.ActionProvider is not GoapActionProvider goapAgent)
                return false;

            if (this.Config.ValidateConditions && !goapAgent.AgentType.AllConditionsMet(goapAgent, this))
            {
                agent.Logger.Warning($"Conditions not met: {this.Config.Name}");
                return false;
            }

            if (this.Config.ValidateTarget && this.Config.RequiresTarget)
            {
                if (data.Target == null)
                {
                    agent.Logger.Warning($"No target found for: {this.Config.Name}");
                    return false;
                }

                if (!data.Target.IsValid())
                {
                    agent.Logger.Warning($"Target became invalid: {this.Config.Name}");
                    return false;
                }
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
