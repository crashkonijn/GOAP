using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GoapAgentLogger : LoggerBase<IMonoGoapActionProvider>
    {
        public override string Name => this.source.name;

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

        private void NoActionFound(IGoalRequest request)
        {
            if (this.config.DebugMode == DebugMode.None)
                return;

            this.Warning($"No action found for goals {string.Join(", ", request.Goals.Select(x => x.GetType().GetGenericTypeName()))}");
        }

        private void GoalStart(IGoal goal)
        {
            if (this.config.DebugMode == DebugMode.None)
                return;

            this.Log($"Goal {goal?.GetType().GetGenericTypeName()} started");
        }

        private void GoalCompleted(IGoal goal)
        {
            if (this.config.DebugMode == DebugMode.None)
                return;

            this.Log($"Goal {goal?.GetType().GetGenericTypeName()} completed");
        }
    }
}
