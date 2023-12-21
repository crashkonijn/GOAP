using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentEvents : IAgentEvents
    {
        private IAgent agent;
        private IAgentTypeEvents typeEvents;

        public void Bind(IAgent agent, IAgentTypeEvents events)
        {
            this.agent = agent;
            this.typeEvents = events;
        }
        
        // Actions
        public event ActionDelegate OnActionStart;
        public void ActionStart(IAction action)
        {
            this.OnActionStart?.Invoke(action);
            this.typeEvents?.ActionStart(this.agent, action);
        }
        
        public event ActionDelegate OnActionStop;
        public void ActionStop(IAction action)
        {
            this.OnActionStop?.Invoke(action);
            this.typeEvents?.ActionStop(this.agent, action);
        }

        public event ActionDelegate OnActionComplete;
        public void ActionComplete(IAction action)
        {
            this.OnActionComplete?.Invoke(action);
            this.typeEvents?.ActionComplete(this.agent, action);
        }

        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoal goal)
        {
            this.OnNoActionFound?.Invoke(goal);
            this.typeEvents?.NoActionFound(this.agent, goal);
        }
        
        // Goals
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoal goal)
        {
            this.OnGoalStart?.Invoke(goal);
            this.typeEvents?.GoalStart(this.agent, goal);
        }

        public event GoalDelegate OnGoalCompleted;
        public void GoalCompleted(IGoal goal)
        {
            this.OnGoalCompleted?.Invoke(goal);
            this.typeEvents?.GoalCompleted(this.agent, goal);
        }

        // Targets
        public event TargetDelegate OnTargetInRange;
        public void TargetInRange(ITarget target)
        {
            this.OnTargetInRange?.Invoke(target);
        }

        public event TargetDelegate OnTargetOutOfRange;
        public void TargetOutOfRange(ITarget target)
        {
            this.OnTargetOutOfRange?.Invoke(target);
        }

        public event TargetRangeDelegate OnTargetChanged;
        public void TargetChanged(ITarget target, bool inRange)
        {
            this.OnTargetChanged?.Invoke(target, inRange);
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
            this.typeEvents?.AgentResolve(this.agent);
        }
    }
}