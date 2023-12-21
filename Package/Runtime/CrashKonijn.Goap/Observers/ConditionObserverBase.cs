using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public abstract class ConditionObserverBase : IConditionObserver
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData worldData)
        {
            this.WorldData = worldData;
        }

        public abstract bool IsMet(ICondition condition);
    }
}