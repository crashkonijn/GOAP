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

        public event AgentActionDelegate OnActionStart;
        public void ActionStart(IAgent agent, IAction action)
        {
            this.OnActionStart?.Invoke(agent, action);
            this.goapEvents?.ActionStart(agent, action);
        }

        public event AgentActionDelegate OnActionStop;
        public void ActionStop(IAgent agent, IAction action)
        {
            this.OnActionStop?.Invoke(agent, action);
            this.goapEvents?.ActionStop(agent, action);
        }

        public event AgentGoalDelegate OnNoActionFound;
        public void NoActionFound(IAgent agent, IGoal goal)
        {
            this.OnNoActionFound?.Invoke(agent, goal);
            this.goapEvents?.NoActionFound(agent, goal);
        }

        public event AgentGoalDelegate OnGoalStart;
        public void GoalStart(IAgent agent, IGoal goal)
        {
            this.OnGoalStart?.Invoke(agent, goal);
            this.goapEvents?.GoalStart(agent, goal);
        }

        public event AgentGoalDelegate OnGoalCompleted;
        public void GoalCompleted(IAgent agent, IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(agent, goal);
            this.goapEvents?.GoalCompleted(agent, goal);
        }

        public event AgentDelegate OnAgentResolve;
        public void AgentResolve(IAgent agent)
        {
            this.OnAgentResolve?.Invoke(agent);
            this.goapEvents?.AgentResolve(agent);
        }

        public event AgentDelegate OnAgentRegistered;
        public void AgentRegistered(IAgent agent)
        {
            this.OnAgentRegistered?.Invoke(agent);
            this.goapEvents?.AgentRegistered(agent);
        }

        public event AgentDelegate OnAgentUnregistered;
        public void AgentUnregistered(IAgent agent)
        {
            this.OnAgentUnregistered?.Invoke(agent);
            this.goapEvents?.AgentUnregistered(agent);
        }
    }
}