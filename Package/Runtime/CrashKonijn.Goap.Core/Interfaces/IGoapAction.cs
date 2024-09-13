using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IGoapAction : IAction, IConnectable, IHasConfig<IActionConfig>
    {
        float GetCost(IActionReceiver agent, IComponentReference references, ITarget target);
    }
}