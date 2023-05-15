using CrashKonijn.Goap.Behaviours;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IAgentEvents
    {
        // Actions
        event ActionDelegate OnActionStart;
        void ActionStart(IActionBase action);
        
        event ActionDelegate OnActionStop;
        void ActionStop(IActionBase action);
        
        event GoalDelegate OnNoActionFound;
        void NoActionFound(IGoalBase goal);
        
        // Goals
        event GoalDelegate OnGoalStart;
        void GoalStart(IGoalBase goal);
        
        event GoalDelegate OnGoalCompleted;
        void GoalCompleted(IGoalBase goal);
        
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