using CrashKonijn.Goap.Behaviours;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IAgentEvents
    {
        event ActionDelegate OnActionStart;
        void ActionStart(IActionBase action);
        
        event ActionDelegate OnActionStop;
        void ActionStop(IActionBase action);
        
        event GoalDelegate OnGoalStart;
        void GoalStart(IGoalBase goal);
        
        event GoalDelegate OnGoalCompleted;
        void GoalCompleted(IGoalBase goal);
        
        event GoalDelegate OnNoActionFound;
        void NoActionFound(IGoalBase goal);
    }
}