using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class GoapEvents : IGoapEvents
    {
        public event GoapAgentActionDelegate OnActionStart;
        public void ActionStart(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionStart?.Invoke(agent, action);
        }

        public event GoapAgentActionDelegate OnActionEnd;
        public void ActionEnd(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionEnd?.Invoke(agent, action);
        }

        public event GoapAgentActionDelegate OnActionStop;
        public void ActionStop(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionStop?.Invoke(agent, action);
        }

        public event GoapAgentActionDelegate OnActionComplete;
        public void ActionComplete(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionComplete?.Invoke(agent, action);
        }

        public event AgentGoalDelegate OnNoActionFound;
        public void NoActionFound(IMonoGoapAgent agent, IGoal goal)
        {
            this.OnNoActionFound?.Invoke(agent, goal);
        }

        public event AgentGoalDelegate OnGoalStart;
        public void GoalStart(IMonoGoapAgent agent, IGoal goal)
        {
            this.OnGoalStart?.Invoke(agent, goal);
        }

        public event AgentGoalDelegate OnGoalCompleted;
        public void GoalCompleted(IMonoGoapAgent agent, IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(agent, goal);
        }

        public event GoapAgentDelegate OnAgentResolve;
        public void AgentResolve(IMonoGoapAgent agent)
        {
            this.OnAgentResolve?.Invoke(agent);
        }

        public event GoapAgentDelegate OnAgentRegistered;
        public void AgentRegistered(IMonoGoapAgent agent)
        {
            this.OnAgentRegistered?.Invoke(agent);
        }

        public event GoapAgentDelegate OnAgentUnregistered;
        public void AgentUnregistered(IMonoGoapAgent agent)
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