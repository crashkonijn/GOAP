namespace CrashKonijn.Goap.Core
{
    public interface IGoal : IConnectable, IHasConfig<IGoalConfig>
    {
        public int GetCost(IWorldData data);
    }
}