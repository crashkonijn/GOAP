using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Core
{
    public interface IGoapAgent : IActionResolver
    {
        float DistanceMultiplier { get; }
        IActionReceiver Agent { get; }
        IAgentType AgentType { get; }
        IGoal CurrentGoal { get; }
        ILocalWorldData WorldData { get; }
        IConnectable[] CurrentPlan { get; }
        IGoapAgentEvents Events { get; }
        ILogger<IMonoGoapActionProvider> Logger { get; }
        Vector3 Position { get; }
        void SetGoal<TGoal>(bool endAction) where TGoal : IGoal;
        void SetGoal(IGoal goal, bool endAction);
        void SetAction(IGoal goal, IGoapAction action, IConnectable[] path);
        void ClearGoal();
        void StopAction(bool resolveAction = true);
        void SetDistanceMultiplierSpeed(float speed);
    }
}