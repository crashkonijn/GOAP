using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IGoapAgentEvents
    {
        void Bind(IMonoGoapActionProvider actionProvider, IAgentTypeEvents events);
        void Bind(IActionReceiver receiver);
        void Unbind();
        event GoalRequestDelegate OnNoActionFound;
        void NoActionFound(IGoalRequest request);
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
