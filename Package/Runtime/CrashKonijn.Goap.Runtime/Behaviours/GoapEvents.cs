using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoapEvents : IGoapEvents
    {
        public event GoapAgentActionDelegate OnActionStart;

        public void ActionStart(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionStart?.Invoke(actionProvider, action);
        }

        public event GoapAgentActionDelegate OnActionEnd;

        public void ActionEnd(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionEnd?.Invoke(actionProvider, action);
        }

        public event GoapAgentActionDelegate OnActionStop;

        public void ActionStop(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionStop?.Invoke(actionProvider, action);
        }

        public event GoapAgentActionDelegate OnActionComplete;

        public void ActionComplete(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionComplete?.Invoke(actionProvider, action);
        }

        public event AgentGoalRequestDelegate OnNoActionFound;

        public void NoActionFound(IMonoGoapActionProvider actionProvider, IGoalRequest request)
        {
            this.OnNoActionFound?.Invoke(actionProvider, request);
        }

        public event AgentGoalDelegate OnGoalStart;

        public void GoalStart(IMonoGoapActionProvider actionProvider, IGoal goal)
        {
            this.OnGoalStart?.Invoke(actionProvider, goal);
        }

        public event AgentGoalDelegate OnGoalCompleted;

        public void GoalCompleted(IMonoGoapActionProvider actionProvider, IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(actionProvider, goal);
        }

        public event GoapAgentDelegate OnAgentResolve;

        public void AgentResolve(IMonoGoapActionProvider actionProvider)
        {
            this.OnAgentResolve?.Invoke(actionProvider);
        }

        public event GoapAgentDelegate OnAgentRegistered;

        public void AgentRegistered(IMonoGoapActionProvider actionProvider)
        {
            this.OnAgentRegistered?.Invoke(actionProvider);
        }

        public event GoapAgentDelegate OnAgentUnregistered;

        public void AgentUnregistered(IMonoGoapActionProvider actionProvider)
        {
            this.OnAgentUnregistered?.Invoke(actionProvider);
        }

        public event AgentTypeDelegate OnAgentTypeRegistered;

        public void AgentTypeRegistered(IAgentType agentType)
        {
            this.OnAgentTypeRegistered?.Invoke(agentType);
        }
    }
}
