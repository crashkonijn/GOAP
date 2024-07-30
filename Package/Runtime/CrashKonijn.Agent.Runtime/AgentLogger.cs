using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public class AgentLogger : LoggerBase<IMonoAgent>
    {
        protected override string Name => this.source.name;

        protected override void RegisterEvents()
        {
            if (this.source == null)
                return;
            
            this.source.Events.OnActionStart += this.ActionStart;
            this.source.Events.OnActionStop += this.ActionStop;
            this.source.Events.OnActionComplete += this.ActionComplete;
            // Todo
            // this.Agent.Events.OnNoActionFound += this.NoActionFound;
            // this.Agent.Events.OnGoalStart += this.GoalStart;
            // this.Agent.Events.OnGoalCompleted += this.GoalCompleted;
        }
        
        protected override void UnregisterEvents()
        {
            if (this.source == null)
                return;
            
            this.source.Events.OnActionStart -= this.ActionStart;
            this.source.Events.OnActionStop -= this.ActionStop;
            this.source.Events.OnActionComplete -= this.ActionComplete;
            // Todo
            // this.Agent.Events.OnNoActionFound -= this.NoActionFound;
            // this.Agent.Events.OnGoalStart -= this.GoalStart;
            // this.Agent.Events.OnGoalCompleted -= this.GoalCompleted;
        }
        
        private void ActionStart(IAction action) => this.Handle($"Action {action?.GetType().GetGenericTypeName()} started", DebugSeverity.Log);
        private void ActionStop(IAction action) => this.Handle($"Action {action?.GetType().GetGenericTypeName()} stopped", DebugSeverity.Log);
        private void ActionComplete(IAction action) => this.Handle($"Action {action?.GetType().GetGenericTypeName()} completed", DebugSeverity.Log);
        // private void NoActionFound(IGoal goal) => this.Handle($"No action found for goal {goal?.GetType().GetGenericTypeName()}", DebugSeverity.Warning);
        // private void GoalStart(IGoal goal) => this.Handle($"Goal {goal?.GetType().GetGenericTypeName()} started", DebugSeverity.Log);
        // private void GoalCompleted(IGoal goal) => this.Handle($"Goal {goal?.GetType().GetGenericTypeName()} completed", DebugSeverity.Log);

    }
}