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
        public void ActionStart(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionStart?.Invoke(actionProvider, action);
            this.goapEvents?.ActionStart(actionProvider, action);
        }
        
        public event GoapAgentActionDelegate OnActionEnd;
        public void ActionEnd(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionEnd?.Invoke(actionProvider, action);
            this.goapEvents?.ActionEnd(actionProvider, action);
        }

        public event GoapAgentActionDelegate OnActionStop;
        public void ActionStop(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionStop?.Invoke(actionProvider, action);
            this.goapEvents?.ActionStop(actionProvider, action);
        }

        public event GoapAgentActionDelegate OnActionComplete;
        public void ActionComplete(IMonoGoapActionProvider actionProvider, IGoapAction action)
        {
            this.OnActionComplete?.Invoke(actionProvider, action);
            this.goapEvents?.ActionComplete(actionProvider, action);
        }

        public event AgentGoalDelegate OnNoActionFound;
        public void NoActionFound(IMonoGoapActionProvider actionProvider, IGoal goal)
        {
            this.OnNoActionFound?.Invoke(actionProvider, goal);
            this.goapEvents?.NoActionFound(actionProvider, goal);
        }

        public event AgentGoalDelegate OnGoalStart;
        public void GoalStart(IMonoGoapActionProvider actionProvider, IGoal goal)
        {
            this.OnGoalStart?.Invoke(actionProvider, goal);
            this.goapEvents?.GoalStart(actionProvider, goal);
        }

        public event AgentGoalDelegate OnGoalCompleted;
        public void GoalCompleted(IMonoGoapActionProvider actionProvider, IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(actionProvider, goal);
            this.goapEvents?.GoalCompleted(actionProvider, goal);
        }

        public event GoapAgentDelegate OnAgentResolve;
        public void AgentResolve(IMonoGoapActionProvider actionProvider)
        {
            this.OnAgentResolve?.Invoke(actionProvider);
            this.goapEvents?.AgentResolve(actionProvider);
        }

        public event GoapAgentDelegate OnAgentRegistered;
        public void AgentRegistered(IMonoGoapActionProvider actionProvider)
        {
            this.OnAgentRegistered?.Invoke(actionProvider);
            this.goapEvents?.AgentRegistered(actionProvider);
        }

        public event GoapAgentDelegate OnAgentUnregistered;
        public void AgentUnregistered(IMonoGoapActionProvider actionProvider)
        {
            this.OnAgentUnregistered?.Invoke(actionProvider);
            this.goapEvents?.AgentUnregistered(actionProvider);
        }
    }
}