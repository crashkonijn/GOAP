using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class GoapEvents : IGoapEvents
    {
        public event AgentActionDelegate OnActionStart;
        public void ActionStart(IAgent agent, IAction action)
        {
            this.OnActionStart?.Invoke(agent, action);
        }

        public event AgentActionDelegate OnActionStop;
        public void ActionStop(IAgent agent, IAction action)
        {
            this.OnActionStop?.Invoke(agent, action);
        }

        public event AgentActionDelegate OnActionComplete;
        public void ActionComplete(IAgent agent, IAction action)
        {
            this.OnActionComplete?.Invoke(agent, action);
        }

        public event AgentGoalDelegate OnNoActionFound;
        public void NoActionFound(IAgent agent, IGoal goal)
        {
            this.OnNoActionFound?.Invoke(agent, goal);
        }

        public event AgentGoalDelegate OnGoalStart;
        public void GoalStart(IAgent agent, IGoal goal)
        {
            this.OnGoalStart?.Invoke(agent, goal);
        }

        public event AgentGoalDelegate OnGoalCompleted;
        public void GoalCompleted(IAgent agent, IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(agent, goal);
        }

        public event AgentDelegate OnAgentResolve;
        public void AgentResolve(IAgent agent)
        {
            this.OnAgentResolve?.Invoke(agent);
        }

        public event AgentDelegate OnAgentRegistered;
        public void AgentRegistered(IAgent agent)
        {
            this.OnAgentRegistered?.Invoke(agent);
        }

        public event AgentDelegate OnAgentUnregistered;
        public void AgentUnregistered(IAgent agent)
        {
            this.OnAgentUnregistered?.Invoke(agent);
        }

        public event AgentTypeDelegate OnAgentTypeRegistered;
        public void AgentTypeRegistered(IAgentType agentType)
        {
            this.OnAgentTypeRegistered?.Invoke(agentType);
        }
    }
}