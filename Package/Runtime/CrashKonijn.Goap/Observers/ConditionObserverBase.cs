using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public interface IConditionObserver : Resolver.Interfaces.IConditionObserver
    {
        void SetWorldData(IWorldData worldData);
    }
    
    public abstract class ConditionObserverBase : IConditionObserver
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData worldData)
        {
            this.WorldData = worldData;
        }
        
        public bool IsMet(Resolver.Interfaces.ICondition condition)
        {
            return this.IsMet((ICondition)condition);
        }

        public abstract bool IsMet(ICondition condition);
    }
}