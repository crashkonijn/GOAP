using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IGoal : IConnectable, IHasConfig<IGoalConfig>
    {
        public float GetCost(IActionReceiver agent, IComponentReference references);
    }
}