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
        }

        public void Bind(IActionReceiver receiver)
        {
            this.Unbind();

            receiver.Events.OnActionStart += this.ActionStart;
            receiver.Events.OnActionEnd += this.ActionEnd;
            receiver.Events.OnActionStop += this.ActionStop;
            receiver.Events.OnActionComplete += this.ActionComplete;
        }

        public void Unbind()
        {
            if (this.actionProvider == null)
                return;

            if (this.actionProvider.Receiver == null)
                return;

            this.actionProvider.Receiver.Events.OnActionStart -= this.ActionStart;
            this.actionProvider.Receiver.Events.OnActionEnd -= this.ActionEnd;
            this.actionProvider.Receiver.Events.OnActionStop -= this.ActionStop;
            this.actionProvider.Receiver.Events.OnActionComplete -= this.ActionComplete;
        }

        public event GoalRequestDelegate OnNoActionFound;

        public void NoActionFound(IGoalRequest goal)
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
