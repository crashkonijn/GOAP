using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoapAgentEvents : IGoapAgentEvents
    {
        private IMonoGoapActionProvider actionProvider;
        private IAgentTypeEvents typeEvents;

        public void Bind(IMonoGoapActionProvider actionProvider, IAgentTypeEvents events)
        {
            this.typeEvents = events;
            this.actionProvider = actionProvider;
            
            actionProvider.Agent.Events.OnActionStart += this.ActionStart;
            actionProvider.Agent.Events.OnActionEnd += this.ActionEnd;
            actionProvider.Agent.Events.OnActionStop += this.ActionStop;
            actionProvider.Agent.Events.OnActionComplete += this.ActionComplete;
        }
        
        public void Unbind()
        {
            this.actionProvider.Agent.Events.OnActionStart -= this.ActionStart;
            this.actionProvider.Agent.Events.OnActionEnd -= this.ActionEnd;
            this.actionProvider.Agent.Events.OnActionStop -= this.ActionStop;
            this.actionProvider.Agent.Events.OnActionComplete -= this.ActionComplete;
        }
        
        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoal goal)
        {
            this.OnNoActionFound?.Invoke(goal);
            this.typeEvents?.NoActionFound(this.actionProvider, goal);
        }
        
        // Goals
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoal goal)
        {
            this.OnGoalStart?.Invoke(goal);
            this.typeEvents?.GoalStart(this.actionProvider, goal);
        }

        public event GoalDelegate OnGoalCompleted;
        public void GoalCompleted(IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(goal);
            this.typeEvents?.GoalCompleted(this.actionProvider, goal);
        }
        
        // General
        public event EmptyDelegate OnResolve;
        public void Resolve()
        {
            this.OnResolve?.Invoke();
            this.typeEvents?.AgentResolve(this.actionProvider);
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
           this.typeEvents.ActionStart(this.actionProvider, goapAction);
        }

        private void ActionEnd(IAction action)
        {
            if (action is not IGoapAction goapAction)
                return;

            this.OnActionEnd?.Invoke(goapAction);
            this.typeEvents.ActionEnd(this.actionProvider, goapAction);
        }

        private void ActionStop(IAction action)
        {
            if (action is not IGoapAction goapAction)
                return;

            this.OnActionStop?.Invoke(goapAction);
            this.typeEvents.ActionStop(this.actionProvider, goapAction);
        }

        private void ActionComplete(IAction action)
        {
            if (action is not IGoapAction goapAction)
                return;

            this.OnActionComplete?.Invoke(goapAction);
            this.typeEvents.ActionComplete(this.actionProvider, goapAction);
        }
    }
}