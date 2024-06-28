using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;
using UnityEngine;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgent
    {
        float DistanceMultiplier { get; }
        List<Type> DisabledActions { get; }
        AgentState State { get; }
        AgentMoveState MoveState { get; }
        [Obsolete("Use ActionState.Action instead.")]
        IAction CurrentAction { get; }
        [Obsolete("Use ActionState.Data instead.")]
        IActionData CurrentActionData { get; }
        [Obsolete("Use ActionState.RunState instead.")]
        IActionRunState RunState { get; }
        IActionState ActionState { get; }
        IAgentEvents Events { get; }
        IDataReferenceInjector Injector { get; }
        IAgentDistanceObserver DistanceObserver { get; }
        IAgentTimers Timers { get; }
        ITarget CurrentTarget { get; }
        ILogger<IMonoAgent> Logger { get; }
        IActionResolver ActionResolver { get; set; }
        Vector3 Position { get; }

        void Initialize();
        void Run();
        
        void SetAction(IActionResolver actionResolver, IAction action, ITarget target);
        void StopAction(bool resolveAction = true);
        void CompleteAction(bool resolveAction = true);
        void ResolveAction();
        void SetDistanceMultiplierSpeed(float speed);
        void EnableAction<TAction>() where TAction : IAction;
        void DisableAction<TAction>() where TAction : IAction;
    }
}