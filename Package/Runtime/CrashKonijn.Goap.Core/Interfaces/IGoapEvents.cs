namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoapEvents
    {
        // Actions
        event GoapAgentActionDelegate OnActionStart;
        void ActionStart(IMonoGoapAgent agent, IGoapAction action);
        
        event GoapAgentActionDelegate OnActionEnd;
        void ActionEnd(IMonoGoapAgent agent, IGoapAction action);
        
        event GoapAgentActionDelegate OnActionStop;
        void ActionStop(IMonoGoapAgent agent, IGoapAction action);
        
        event GoapAgentActionDelegate OnActionComplete;
        void ActionComplete(IMonoGoapAgent agent, IGoapAction action);
        
        event AgentGoalDelegate OnNoActionFound;
        void NoActionFound(IMonoGoapAgent agent, IGoal goal);
        
        // Goals
        event AgentGoalDelegate OnGoalStart;
        void GoalStart(IMonoGoapAgent agent, IGoal goal);
        
        event AgentGoalDelegate OnGoalCompleted;
        void GoalCompleted(IMonoGoapAgent agent, IGoal goal);
        
        // General
        event GoapAgentDelegate OnAgentResolve;
        void AgentResolve(IMonoGoapAgent agent);
        
        event GoapAgentDelegate OnAgentRegistered;
        void AgentRegistered(IMonoGoapAgent agent);
        
        event GoapAgentDelegate OnAgentUnregistered;
        void AgentUnregistered(IMonoGoapAgent agent);
        
        // Agent Types
        event AgentTypeDelegate OnAgentTypeRegistered;
        void AgentTypeRegistered(IAgentType agentType);
    }
}