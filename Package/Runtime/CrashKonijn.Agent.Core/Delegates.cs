namespace CrashKonijn.Agent.Core
{
    public delegate void ActionDelegate(IAction action);

    public delegate void AgentActionDelegate(IAgent agent, IAction action);

    public delegate void TargetDelegate(ITarget target);

    public delegate void TargetRangeDelegate(ITarget target, bool inRange);

    public delegate void AgentDelegate(IAgent agent);

    public delegate void EmptyDelegate();
}
