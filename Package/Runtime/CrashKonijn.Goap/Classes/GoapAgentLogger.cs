using CrashKonijn.Agent;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Classes
{
    public class GoapAgentLogger : LoggerBase<IMonoGoapActionProvider>
    {
        protected override string Name => this.source.name;

        protected override void RegisterEvents()
        {
            if (this.source == null)
                return;
            
            // Todo
            this.source.Events.OnNoActionFound += this.NoActionFound;
            this.source.Events.OnGoalStart += this.GoalStart;
            this.source.Events.OnGoalCompleted += this.GoalCompleted;
        }
        
        protected override void UnregisterEvents()
        {
            if (this.source == null)
                return;
            
            // Todo
            this.source.Events.OnNoActionFound -= this.NoActionFound;
            this.source.Events.OnGoalStart -= this.GoalStart;
            this.source.Events.OnGoalCompleted -= this.GoalCompleted;
        }
        
        private void NoActionFound(IGoal goal) => this.Handle($"No action found for goal {goal?.GetType().GetGenericTypeName()}", DebugSeverity.Warning);
        private void GoalStart(IGoal goal) => this.Handle($"Goal {goal?.GetType().GetGenericTypeName()} started", DebugSeverity.Log);
        private void GoalCompleted(IGoal goal) => this.Handle($"Goal {goal?.GetType().GetGenericTypeName()} completed", DebugSeverity.Log);

    }
}