using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Core
{
    public delegate void ActionDelegate(IAction action);
    public delegate void AgentActionDelegate(IAgent agent, IAction action);
    public delegate void GoalDelegate(IGoal goal);
    public delegate void AgentGoalDelegate(IAgent agent, IGoal goal);
    public delegate void TargetDelegate(ITarget target);
    public delegate void TargetRangeDelegate(ITarget target, bool inRange);
    public delegate void AgentDelegate(IAgent agent);
    public delegate void AgentTypeDelegate(IAgentType agentType);
    public delegate void EmptyDelegate();
}