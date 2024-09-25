using CrashKonijn.Agent.Core;

namespace CrashKonijn.Goap.Core
{
    public interface IGoal : IConnectable, IHasConfig<IGoalConfig>
    {
        public int Index { get; set; }
        public float GetCost(IActionReceiver agent, IComponentReference references);
    }
}