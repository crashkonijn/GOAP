using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class GoapAgentEvents : IGoapAgentEvents
    {
        private IMonoGoapAgent agent;
        private IAgentTypeEvents typeEvents;

        public void Bind(IMonoGoapAgent agent, IAgentTypeEvents events)
        {
            this.typeEvents = events;
            this.agent = agent;
            
            agent.Agent.Events.OnActionStart += this.ActionStart;
            agent.Agent.Events.OnActionEnd += this.ActionEnd;
            agent.Agent.Events.OnActionStop += this.ActionStop;
            agent.Agent.Events.OnActionComplete += this.ActionComplete;
        }
        
        public void Unbind()
        {
            this.agent.Agent.Events.OnActionStart -= this.ActionStart;
            this.agent.Agent.Events.OnActionEnd -= this.ActionEnd;
            this.agent.Agent.Events.OnActionStop -= this.ActionStop;
            this.agent.Agent.Events.OnActionComplete -= this.ActionComplete;
        }
        
        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoal goal)
        {
            this.OnNoActionFound?.Invoke(goal);
            this.typeEvents?.NoActionFound(this.agent, goal);
        }
        
        // Goals
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoal goal)
        {
            this.OnGoalStart?.Invoke(goal);
            this.typeEvents?.GoalStart(this.agent, goal);
        }

        public event GoalDelegate OnGoalCompleted;
        public void GoalCompleted(IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(goal);
            this.typeEvents?.GoalCompleted(this.agent, goal);
        }
        
        // General
        public event EmptyDelegate OnResolve;
        public void Resolve()
        {
            this.OnResolve?.Invoke();
            this.typeEvents?.AgentResolve(this.agent);
        }
        
        // Agent events
        public event GoapActionDelegate OnActionStart;
        public event GoapActionDelegate OnActionEnd;
        public event GoapActionDelegate OnActionStop;
        public event GoapActionDelegate OnActionComplete;
        
        // Pass through
        private void ActionStart(IAction action)
        {
           if (action is not IGoapAction goapAction)
               return;

           this.OnActionStart?.Invoke(goapAction);
           this.typeEvents.ActionStart(this.agent, goapAction);
        }

        private void ActionEnd(IAction action)
        {
            if (action is not IGoapAction goapAction)
                return;

            this.OnActionEnd?.Invoke(goapAction);
            this.typeEvents.ActionEnd(this.agent, goapAction);
        
        }

        private void ActionStop(IAction action)
        {
            if (action is not IGoapAction goapAction)
                return;

            this.OnActionStop?.Invoke(goapAction);
            this.typeEvents.ActionStop(this.agent, goapAction);
        }

        private void ActionComplete(IAction action)
        {
            if (action is not IGoapAction goapAction)
                return;

            this.OnActionComplete?.Invoke(goapAction);
            this.typeEvents.ActionComplete(this.agent, goapAction);
        }
    }
}