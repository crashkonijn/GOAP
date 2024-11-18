namespace CrashKonijn.Goap.Core
{
    public interface IConditionObserver
    {
        bool IsMet(ICondition condition);
        void SetWorldData(IWorldData worldData);
    }
}
