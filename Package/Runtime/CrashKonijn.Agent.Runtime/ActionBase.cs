using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public abstract class AgentActionBase<TActionData, TActionProperties>
        where TActionData : IActionData, new()
        where TActionProperties : class, IActionProperties, new()
    {
        /// <summary>
        ///     Gets the action data.
        /// </summary>
        /// <returns>The action data.</returns>
        public IActionData GetData()
        {
            return this.CreateData();
        }

        /// <summary>
        ///     Creates a new instance of action data.
        /// </summary>
        /// <returns>The created action data.</returns>
        public virtual TActionData CreateData()
        {
            return new TActionData();
        }

        /// <summary>
        ///     Gets the action properties.
        /// </summary>
        public abstract TActionProperties Properties { get; }

        /// <summary>
        ///     Called when the action is created.
        /// </summary>
        public virtual void Created() { }

        /// <summary>
        ///     Called when the action is started (when it is assigned to an agent).
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public void Start(IMonoAgent agent, IActionData data) => this.Start(agent, (TActionData) data);

        /// <summary>
        ///     Called when the action is started (when it is assigned to an agent).
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public virtual void Start(IMonoAgent agent, TActionData data) { }

        /// <summary>
        ///     Called once before performing the action. Don't override this method, override the other BeforePerform method
        ///     instead.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public void BeforePerform(IMonoAgent agent, IActionData data) => this.BeforePerform(agent, (TActionData) data);

        /// <summary>
        ///     Called once before performing the action. Override this method to implement custom logic.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public virtual void BeforePerform(IMonoAgent agent, TActionData data) { }

        /// <summary>
        ///     Performs the action. Don't override this method, override the other Perform method instead.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        /// <param name="context">The action context.</param>
        /// <returns>The action run state.</returns>
        public IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context) => this.Perform(agent, (TActionData) data, context);

        /// <summary>
        ///     Performs the action with the specified action data. Use this method to implement custom logic.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        /// <param name="context">The action context.</param>
        /// <returns>The action run state.</returns>
        public abstract IActionRunState Perform(IMonoAgent agent, TActionData data, IActionContext context);

        /// <summary>
        ///     Called when the action ends. This is called after the action is completed or stopped.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public virtual void End(IMonoAgent agent, TActionData data) { }

        /// <summary>
        ///     Called when the action is stopped. Don't override this method, override the other Stop method instead.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public void Stop(IMonoAgent agent, IActionData data)
        {
            this.Stop(agent, (TActionData) data);
            this.End(agent, (TActionData) data);
        }

        /// <summary>
        ///     Called when the action is stopped. Use this method to implement custom logic.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public virtual void Stop(IMonoAgent agent, TActionData data) { }

        /// <summary>
        ///     Called when the action is completed. Don't override this method, override the other Complete method instead.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public void Complete(IMonoAgent agent, IActionData data)
        {
            this.Complete(agent, (TActionData) data);
            this.End(agent, (TActionData) data);
        }

        /// <summary>
        ///     Called when the action is completed. Use this method to implement custom logic.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <param name="data">The action data.</param>
        public virtual void Complete(IMonoAgent agent, TActionData data) { }
    }
}
