namespace CrashKonijn.Agent.Core
{
    public interface IAgentEvents
    {
        // Actions
        event ActionDelegate OnActionStart;
        void ActionStart(IAction action);
        
        event ActionDelegate OnActionEnd;
        void ActionEnd(IAction action);
        
        event ActionDelegate OnActionStop;
        void ActionStop(IAction action);
        
        event ActionDelegate OnActionComplete;
        void ActionComplete(IAction action);
        
        // Targets
        event TargetDelegate OnTargetInRange;
        void TargetInRange(ITarget target);
        
        event TargetDelegate OnTargetNotInRange;
        void TargetNotInRange(ITarget target);
        
        event TargetRangeDelegate OnTargetChanged;
        void TargetChanged(ITarget target, bool inRange);

        event TargetDelegate OnMove;
        void Move(ITarget target);
    }
}