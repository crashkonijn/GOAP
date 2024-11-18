using System;

namespace CrashKonijn.Agent.Core
{
    public interface IAgent : IActionReceiver
    {
        AgentState State { get; }
        AgentMoveState MoveState { get; }

        [Obsolete("Use ActionState.Action instead.")]
        IAction CurrentAction { get; }

        [Obsolete("Use ActionState.Data instead.")]
        IActionData CurrentActionData { get; }

        [Obsolete("Use ActionState.RunState instead.")]
        IActionRunState RunState { get; }

        IAgentDistanceObserver DistanceObserver { get; }
        ITarget CurrentTarget { get; }

        void Initialize();
        void Run();

        void CompleteAction(bool resolveAction = true);
        void ResolveAction();

        [Obsolete("Use GoapActionProvider.CurrentPlan.Goal instead")]
        object CurrentGoal { get; set; }

        [Obsolete("Use GoapActionProvider.AgentType instead")]
        object GoapSet { get; set; }
    }
}
