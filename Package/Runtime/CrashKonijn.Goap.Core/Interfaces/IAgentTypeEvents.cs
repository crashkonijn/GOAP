namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentTypeEvents
    {
        void Bind(IGoapEvents events);
        
        // Actions
        event AgentActionDelegate OnActionStart;
        void ActionStart(IAgent agent, IAction action);
        
        event AgentActionDelegate OnActionEnd;
        void ActionEnd(IAgent agent, IAction action);
        
        event AgentActionDelegate OnActionStop;
        void ActionStop(IAgent agent, IAction action);
        
        event AgentActionDelegate OnActionComplete;
        void ActionComplete(IAgent agent, IAction action);
        
        event AgentGoalDelegate OnNoActionFound;
        void NoActionFound(IAgent agent, IGoal goal);
        
        // Goals
        event AgentGoalDelegate OnGoalStart;
        void GoalStart(IAgent agent, IGoal goal);
        
        event AgentGoalDelegate OnGoalCompleted;
        void GoalCompleted(IAgent agent, IGoal goal);
        
        // General
        event AgentDelegate OnAgentResolve;
        void AgentResolve(IAgent agent);
        
        event AgentDelegate OnAgentRegistered;
        void AgentRegistered(IAgent agent);
        
        event AgentDelegate OnAgentUnregistered;
        void AgentUnregistered(IAgent agent);
    }
}