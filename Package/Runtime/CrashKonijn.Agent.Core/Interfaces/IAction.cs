namespace CrashKonijn.Agent.Core
{
    public interface IAction
    {
        ActionMoveMode GetMoveMode(IMonoAgent agent);
        float GetStoppingDistance();
        bool IsInRange(IMonoAgent agent, float distance, IActionData data, IComponentReference references);
        IActionData GetData();

        /// <summary>
        ///     Called when the action is created
        /// </summary>
        void Created();

        /// <summary>
        ///     Check if the action is valid
        /// </summary>
        bool IsValid(IActionReceiver agent, IActionData data);

        /// <summary>
        ///     Called when the action is started/assigned to an agent
        /// </summary>
        void Start(IMonoAgent agent, IActionData data);

        /// <summary>
        ///     Called the first time the action is performed
        /// </summary>
        void BeforePerform(IMonoAgent agent, IActionData data);

        /// <summary>
        ///     Called every frame while the action is being performed
        /// </summary>
        IActionRunState Perform(IMonoAgent agent, IActionData data, IActionContext context);

        /// <summary>
        ///     Called when the action is stopped (forced or otherwise)
        /// </summary>
        void Stop(IMonoAgent agent, IActionData data);

        /// <summary>
        ///     Called when the action is completed
        /// </summary>
        void Complete(IMonoAgent agent, IActionData data);

        bool IsExecutable(IActionReceiver agent, bool conditionsMet);
        bool IsEnabled(IActionReceiver agent);
        void Enable();
        void Disable(IActionDisabler disabler);
    }
}
