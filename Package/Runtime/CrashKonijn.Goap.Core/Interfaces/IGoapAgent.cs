namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoapAgent : IActionResolver
    {
        IMonoAgent Agent { get; }
        IAgentType AgentType { get; }
        IGoal CurrentGoal { get; }
        ILocalWorldData WorldData { get; }
        IConnectable[] CurrentPlan { get; }
        IGoapAgentEvents Events { get; }
        ILogger<IMonoGoapAgent> Logger { get; }
        void SetGoal<TGoal>(bool endAction) where TGoal : IGoal;
        void SetGoal(IGoal goal, bool endAction);
        void SetAction(IGoapAction action, IConnectable[] path, ITarget target);
        void ClearGoal();
        void StopAction(bool resolveAction = true);
    }
}