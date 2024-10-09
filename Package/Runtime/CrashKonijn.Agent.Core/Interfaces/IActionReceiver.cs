using UnityEngine;

namespace CrashKonijn.Agent.Core
{
    public interface IActionReceiver
    {
        IDataReferenceInjector Injector { get; }
        IActionState ActionState { get; }
        IAgentTimers Timers { get; }
        ILogger<IMonoAgent> Logger { get; }
        IAgentEvents Events { get; }
        Transform Transform { get; }
        IActionProvider ActionProvider { get; }
        void SetAction(IActionProvider actionProvider, IAction action, ITarget target);
        void StopAction(bool resolveAction = true);
    }
}