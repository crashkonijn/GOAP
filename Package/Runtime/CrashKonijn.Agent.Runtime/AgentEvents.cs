using System;
using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public class AgentEvents : IAgentEvents
    {
        // Actions
        public event ActionDelegate OnActionStart;

        public void ActionStart(IAction action)
        {
            this.OnActionStart?.Invoke(action);
        }

        public event ActionDelegate OnActionEnd;

        public void ActionEnd(IAction action)
        {
            this.OnActionEnd?.Invoke(action);
        }

        public event ActionDelegate OnActionStop;

        public void ActionStop(IAction action)
        {
            this.OnActionStop?.Invoke(action);

            this.ActionEnd(action);
        }

        public event ActionDelegate OnActionComplete;

        public void ActionComplete(IAction action)
        {
            this.OnActionComplete?.Invoke(action);

            this.ActionEnd(action);
        }

        // Targets
        public event TargetDelegate OnTargetInRange;

        public void TargetInRange(ITarget target)
        {
            this.OnTargetInRange?.Invoke(target);
        }

        public event TargetDelegate OnTargetNotInRange;

        public void TargetNotInRange(ITarget target)
        {
            this.OnTargetNotInRange?.Invoke(target);
        }

        public event TargetRangeDelegate OnTargetChanged;

        public void TargetChanged(ITarget target, bool inRange)
        {
            this.OnTargetChanged?.Invoke(target, inRange);
        }

        public event EmptyDelegate OnTargetLost;

        public void TargetLost()
        {
            this.OnTargetLost?.Invoke();
        }

        public event TargetDelegate OnMove;

        public void Move(ITarget target)
        {
            this.OnMove?.Invoke(target);
        }

        // General
        public event EmptyDelegate OnResolve;

        public void Resolve()
        {
            this.OnResolve?.Invoke();
        }

        [Obsolete("Use GoapActionProvider.Events.OnNoActionFound instead")]
        public event ActionDelegate OnNoActionFound;

        [Obsolete("Use GoapActionProvider.Events.OnNoActionFound instead")]
        public event ActionDelegate OnGoalCompleted;

        [Obsolete("Use TargetNotInRange instead")]
        public event TargetDelegate OnTargetOutOfRange;
    }
}
