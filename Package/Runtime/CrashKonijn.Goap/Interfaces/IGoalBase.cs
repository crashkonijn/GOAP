using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoalBase : IAction, IHasConfig<IGoalConfig>
    {
        public int GetCost(IWorldData data);
    }
}