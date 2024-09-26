namespace CrashKonijn.Goap.Core
{
    public interface IGoapActionProviderEvent
    {
        void NoActionFound(IGoal goal);

        // Goals
        event GoalDelegate OnGoalStart;
        void GoalStart(IGoal goal);

        event GoalDelegate OnGoalCompleted;
        void GoalCompleted(IGoal goal);
    }
}
