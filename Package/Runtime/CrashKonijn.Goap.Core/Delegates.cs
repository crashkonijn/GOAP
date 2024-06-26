using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Core
{
    public delegate void GoalDelegate(IGoal goal);
    public delegate void AgentGoalDelegate(IMonoGoapAgent agent, IGoal goal);
    public delegate void AgentTypeDelegate(IAgentType agentType);
    public delegate void GoapAgentDelegate(IMonoGoapAgent agent);
    public delegate void GoapActionDelegate(IGoapAction action);
    public delegate void GoapAgentActionDelegate(IMonoGoapAgent agent, IGoapAction action);
}