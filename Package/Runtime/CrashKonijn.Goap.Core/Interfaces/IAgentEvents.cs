namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentEvents
    {
        // Actions
        event ActionDelegate OnActionStart;
        void ActionStart(IAction action);
        
        event ActionDelegate OnActionStop;
        void ActionStop(IAction action);
        
        event GoalDelegate OnNoActionFound;
        void NoActionFound(IGoal goal);
        
        // Goals
        event GoalDelegate OnGoalStart;
        void GoalStart(IGoal goal);
        
        event GoalDelegate OnGoalCompleted;
        void GoalCompleted(IGoal goal);
        
        // Targets
        event TargetDelegate OnTargetInRange;
        void TargetInRange(ITarget target);
        
        event TargetDelegate OnTargetOutOfRange;
        void TargetOutOfRange(ITarget target);
        
        event TargetRangeDelegate OnTargetChanged;
        void TargetChanged(ITarget target, bool inRange);

        event TargetDelegate OnMove;
        void Move(ITarget target);
    }
}