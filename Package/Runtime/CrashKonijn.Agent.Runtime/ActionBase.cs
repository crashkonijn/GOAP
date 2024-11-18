using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public abstract class AgentActionBase<TActionData, TActionProperties>
        where TActionData : IActionData, new()
        where TActionProperties : class, IActionProperties, new()
    {
        private IActionDisabler disabler;

        public IActionData GetData()
        {
            return this.CreateData();
        }

        public virtual TActionData CreateData()
        {
            return new TActionData();
        }

        public abstract TActionProperties Properties { get; }

        public virtual void Created() { }

        public void Start(IMonoAgent agent, IActionData data) => this.Start(agent, (TActionData) data);

        public virtual void Start(IMonoAgent agent, TActionData data) { }

        public bool IsEnabled(IActionReceiver agent)
        {
            return this.IsEnabled(agent, agent.Injector);
        }

        public void Enable()
        {
            this.disabler = null;
        }

        public void Disable(IActionDisabler disabler)
        {
            this.disabler = disabler;
        }

        public virtual bool IsEnabled(IActionReceiver receiver, IComponentReference references)
        {
            if (this.disabler == null)
                return true;

            if (receiver is not IMonoAgent agent)
                return true;

            if (this.disabler.IsDisabled(agent))
                return false;

            this.Enable();
            return true;
        }

        public void BeforePerform(IMonoAgent agent, IActionData data) => this.BeforePerform(agent, (TActionData) data);

        public virtual void BeforePerform(IMonoAgent agent, TActionData data) { }

        public IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context) => this.Perform(agent, (TActionData) data, context);

        public abstract IActionRunState Perform(IMonoAgent agent, TActionData data, IActionContext context);

        public virtual void End(IMonoAgent agent, TActionData data) { }

        public void Stop(IMonoAgent agent, IActionData data)
        {
            this.Stop(agent, (TActionData) data);
            this.End(agent, (TActionData) data);
        }

        public virtual void Stop(IMonoAgent agent, TActionData data) { }

        public void Complete(IMonoAgent agent, IActionData data)
        {
            this.Complete(agent, (TActionData) data);
            this.End(agent, (TActionData) data);
        }

        public virtual void Complete(IMonoAgent agent, TActionData data) { }
    }
}
