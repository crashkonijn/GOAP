namespace CrashKonijn.Agent.Core
{
    public enum AgentState
    {
        NoAction,
        StartingAction,
        PerformingAction,
        MovingToTarget,
        MovingWhilePerformingAction,
    }
}