namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoapAgentEvents
    {
        void Bind(IMonoGoapAgent agent, IAgentTypeEvents events);
        void Unbind();
        event GoalDelegate OnNoActionFound;
        void NoActionFound(IGoal goal);
        event GoalDelegate OnGoalStart;
        void GoalStart(IGoal goal);
        
        event GoalDelegate OnGoalCompleted;
        void GoalCompleted(IGoal goal);
        
        event EmptyDelegate OnResolve;
        void Resolve();
        
        event GoapActionDelegate OnActionStart;
        event GoapActionDelegate OnActionEnd;
        event GoapActionDelegate OnActionStop;
        event GoapActionDelegate OnActionComplete;
    }
}