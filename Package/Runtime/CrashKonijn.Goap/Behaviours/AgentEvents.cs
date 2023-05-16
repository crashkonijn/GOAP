using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentEvents : IAgentEvents
    {
        // Actions
        public event ActionDelegate OnActionStart;
        public void ActionStart(IActionBase action)
        {
            this.OnActionStart?.Invoke(action);
        }
        
        public event ActionDelegate OnActionStop;
        public void ActionStop(IActionBase action)
        {
            this.OnActionStop?.Invoke(action);
        }

        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoalBase goal)
        {
            this.OnNoActionFound?.Invoke(goal);
        }
        
        // Goals
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoalBase goal)
        {
            this.OnGoalStart?.Invoke(goal);
        }

        public event GoalDelegate OnGoalCompleted;
        public void GoalCompleted(IGoalBase goal)
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