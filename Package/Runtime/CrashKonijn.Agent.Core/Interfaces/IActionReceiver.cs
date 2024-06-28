using UnityEngine;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IActionReceiver
    {
        IDataReferenceInjector Injector { get; }
        IActionState ActionState { get; }
        IAgentTimers Timers { get; }
        ILogger<IMonoAgent> Logger { get; }
        Transform Transform { get; }
        void SetAction(IActionResolver actionResolver, IAction action, ITarget target);
        void StopAction(bool resolveAction = true);
    }
}