namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoal : IConnectable, IHasConfig<IGoalConfig>
    {
        public int GetCost(IWorldData data);
    }
}