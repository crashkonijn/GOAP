using System;
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
 
    }

    public abstract class ActionBase<TActionData, TActionProperties>
        where TActionData : IActionData, new()
        where TActionProperties : class, IActionProperties, new()
    {
        public IActionData GetData()
        {
            return this.CreateData();
        }

        public virtual TActionData CreateData()
        {
            return new TActionData();
        }
        
        public abstract TActionProperties Properties { get; }

        public void Start(IMonoAgent agent, IActionData data) => this.Start(agent, (TActionData) data);
        
        public abstract void Start(IMonoAgent agent, TActionData data);

        public virtual bool IsValid(IMonoAgent agent, TActionData data)
        {
            return true;
        }
        
        public bool IsEnabled(IMonoAgent agent)
        {
            return this.IsEnabled(agent, agent.Injector);
        }
        
        public virtual bool IsEnabled(IMonoAgent agent, IComponentReference references)
        {
            return !agent.DisabledActions.Contains(this.GetType());
        }
        
        public void BeforePerform(IMonoAgent agent, IActionData data) => this.BeforePerform(agent, (TActionData) data);
        
        public virtual void BeforePerform(IMonoAgent agent, TActionData data) {}

        public IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context) => this.Perform(agent, (TActionData) data, context);

        public abstract IActionRunState Perform(IMonoAgent agent, TActionData data, IActionContext context);
        public virtual void End(IMonoAgent agent, TActionData data) {}

        public void Stop(IMonoAgent agent, IActionData data)
        {
            this.Stop(agent, (TActionData)data);
            this.End(agent, (TActionData)data);
        }
        
        public virtual void Stop(IMonoAgent agent, TActionData data) {}

        public void Complete(IMonoAgent agent, IActionData data)
        {
            this.Complete(agent, (TActionData) data);
            this.End(agent, (TActionData) data);
        }
        
        public virtual void Complete(IMonoAgent agent, TActionData data) {}
    }

    public abstract class ActionBase : IAction
    {
        public abstract float GetCost(IMonoAgent agent, IComponentReference references);
        public abstract ActionMoveMode GetMoveMode(IMonoAgent agent);
        public abstract float GetInRange(IMonoAgent agent, IActionData data);
        public abstract bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references);
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