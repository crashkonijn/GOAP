using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Core
{
    public delegate void GoalDelegate(IGoal goal);
    public delegate void AgentGoalDelegate(IMonoGoapActionProvider actionProvider, IGoal goal);
    public delegate void AgentTypeDelegate(IAgentType agentType);
    public delegate void GoapAgentDelegate(IMonoGoapActionProvider actionProvider);
    public delegate void GoapActionDelegate(IGoapAction action);
    public delegate void GoapAgentActionDelegate(IMonoGoapActionProvider actionProvider, IGoapAction action);
}