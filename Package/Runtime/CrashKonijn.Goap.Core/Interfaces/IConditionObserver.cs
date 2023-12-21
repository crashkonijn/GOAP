
namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IConditionObserver
    {
        bool IsMet(ICondition condition);
        void SetWorldData(IWorldData worldData);
    }
}