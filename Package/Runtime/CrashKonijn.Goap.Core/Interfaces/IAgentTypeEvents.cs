namespace CrashKonijn.Goap.Core
{
    public interface IAgentTypeEvents
    {
        void Bind(IGoapEvents events);

        // Actions
        event GoapAgentActionDelegate OnActionStart;
        void ActionStart(IMonoGoapActionProvider actionProvider, IGoapAction action);

        event GoapAgentActionDelegate OnActionEnd;
        void ActionEnd(IMonoGoapActionProvider actionProvider, IGoapAction action);

        event GoapAgentActionDelegate OnActionStop;
        void ActionStop(IMonoGoapActionProvider actionProvider, IGoapAction action);

        event GoapAgentActionDelegate OnActionComplete;
        void ActionComplete(IMonoGoapActionProvider actionProvider, IGoapAction action);

        event AgentGoalRequestDelegate OnNoActionFound;
        void NoActionFound(IMonoGoapActionProvider actionProvider, IGoalRequest request);

        // Goals
        event AgentGoalDelegate OnGoalStart;
        void GoalStart(IMonoGoapActionProvider actionProvider, IGoal goal);

        event AgentGoalDelegate OnGoalCompleted;
        void GoalCompleted(IMonoGoapActionProvider actionProvider, IGoal goal);

        // General
        event GoapAgentDelegate OnAgentResolve;
        void AgentResolve(IMonoGoapActionProvider actionProvider);

        event GoapAgentDelegate OnAgentRegistered;
        void AgentRegistered(IMonoGoapActionProvider actionProvider);

        event GoapAgentDelegate OnAgentUnregistered;
        void AgentUnregistered(IMonoGoapActionProvider actionProvider);
    }
}
