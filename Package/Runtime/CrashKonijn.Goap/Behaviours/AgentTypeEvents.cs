using System.Reflection;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentTypeEvents : IAgentTypeEvents
    {
        private IGoapEvents goapEvents;

        public void Bind(IGoapEvents events)
        {
            this.goapEvents = events;
        }

        public event GoapAgentActionDelegate OnActionStart;
        public void ActionStart(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionStart?.Invoke(agent, action);
            this.goapEvents?.ActionStart(agent, action);
        }
        
        public event GoapAgentActionDelegate OnActionEnd;
        public void ActionEnd(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionEnd?.Invoke(agent, action);
            this.goapEvents?.ActionEnd(agent, action);
        }

        public event GoapAgentActionDelegate OnActionStop;
        public void ActionStop(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionStop?.Invoke(agent, action);
            this.goapEvents?.ActionStop(agent, action);
        }

        public event GoapAgentActionDelegate OnActionComplete;
        public void ActionComplete(IMonoGoapAgent agent, IGoapAction action)
        {
            this.OnActionComplete?.Invoke(agent, action);
            this.goapEvents?.ActionComplete(agent, action);
        }

        public event AgentGoalDelegate OnNoActionFound;
        public void NoActionFound(IMonoGoapAgent agent, IGoal goal)
        {
            this.OnNoActionFound?.Invoke(agent, goal);
            this.goapEvents?.NoActionFound(agent, goal);
        }

        public event AgentGoalDelegate OnGoalStart;
        public void GoalStart(IMonoGoapAgent agent, IGoal goal)
        {
            this.OnGoalStart?.Invoke(agent, goal);
            this.goapEvents?.GoalStart(agent, goal);
        }

        public event AgentGoalDelegate OnGoalCompleted;
        public void GoalCompleted(IMonoGoapAgent agent, IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(agent, goal);
            this.goapEvents?.GoalCompleted(agent, goal);
        }

        public event GoapAgentDelegate OnAgentResolve;
        public void AgentResolve(IMonoGoapAgent agent)
        {
            this.OnAgentResolve?.Invoke(agent);
            this.goapEvents?.AgentResolve(agent);
        }

        public event GoapAgentDelegate OnAgentRegistered;
        public void AgentRegistered(IMonoGoapAgent agent)
        {
            this.OnAgentRegistered?.Invoke(agent);
            this.goapEvents?.AgentRegistered(agent);
        }

        public event GoapAgentDelegate OnAgentUnregistered;
        public void AgentUnregistered(IMonoGoapAgent agent)
        {
            this.OnAgentUnregistered?.Invoke(agent);
            this.goapEvents?.AgentUnregistered(agent);
        }
    }
}