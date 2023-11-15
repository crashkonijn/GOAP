using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentEvents : IAgentEvents
    {
        // Actions
        public event ActionDelegate OnActionStart;
        public void ActionStart(IAction action)
        {
            this.OnActionStart?.Invoke(action);
        }
        
        public event ActionDelegate OnActionStop;
        public void ActionStop(IAction action)
        {
            this.OnActionStop?.Invoke(action);
        }

        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoal goal)
        {
            this.OnNoActionFound?.Invoke(goal);
        }
        
        // Goals
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoal goal)
        {
            this.OnGoalStart?.Invoke(goal);
        }

        public event GoalDelegate OnGoalCompleted;
        public void GoalCompleted(IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(goal);
        }

        // Targets
        public event TargetDelegate OnTargetInRange;
        public void TargetInRange(ITarget target)
        {
            this.OnTargetInRange?.Invoke(target);
        }

        public event TargetDelegate OnTargetOutOfRange;
        public void TargetOutOfRange(ITarget target)
        {
            this.OnTargetOutOfRange?.Invoke(target);
        }

        public event TargetRangeDelegate OnTargetChanged;
        public void TargetChanged(ITarget target, bool inRange)
        {
            this.OnTargetChanged?.Invoke(target, inRange);
        }

        public event TargetDelegate OnMove;
        public void Move(ITarget target)
        {
            this.OnMove?.Invoke(target);
        }
    }
}