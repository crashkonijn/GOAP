using System;

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

        event EmptyDelegate OnTargetLost;
        void TargetLost();

        event TargetDelegate OnMove;
        void Move(ITarget target);

        [Obsolete("Use GoapActionProvider.Events.OnNoActionFound instead")]
        event ActionDelegate OnNoActionFound;

        [Obsolete("Use GoapActionProvider.Events.OnNoActionFound instead")]
        event ActionDelegate OnGoalCompleted;

        [Obsolete("Use TargetNotInRange instead")]
        event TargetDelegate OnTargetOutOfRange;
    }
}
