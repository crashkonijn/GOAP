using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;
using UnityEngine;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgent
    {
        float DistanceMultiplier { get; }
        DebugMode DebugMode { get; }
        int MaxLogSize { get; }
        List<Type> DisabledActions { get; }
        AgentState State { get; }
        AgentMoveState MoveState { get; }
        IAgentType AgentType { get; }
        IGoal CurrentGoal { get; }
        [Obsolete("Use ActionState.Action instead.")]
        IAction CurrentAction { get; }
        [Obsolete("Use ActionState.Data instead.")]
        IActionData CurrentActionData { get; }
        [Obsolete("Use ActionState.RunState instead.")]
        IActionRunState RunState { get; }
        IActionState ActionState { get; }
        ILocalWorldData WorldData { get; }
        IConnectable[] CurrentPlan { get; }
        IAgentEvents Events { get; }
        IDataReferenceInjector Injector { get; }
        IAgentDistanceObserver DistanceObserver { get; }
        IAgentTimers Timers { get; }
        ITarget CurrentTarget { get; }
        ILogger Logger { get; }
        Vector3 Position { get; }

        void Run();
        
        void ClearGoal();
        void SetGoal<TGoal>(bool endAction) where TGoal : IGoal;
        void SetGoal(IGoal goal, bool endAction);
        void SetAction(IAction action, IConnectable[] path, ITarget target);
        void StopAction(bool resolveAction = true);
        void CompleteAction(bool resolveAction = true);
        void ResolveAction();
        void SetDistanceMultiplierSpeed(float speed);
        void EnableAction<TAction>() where TAction : IAction;
        void DisableAction<TAction>() where TAction : IAction;
    }
}