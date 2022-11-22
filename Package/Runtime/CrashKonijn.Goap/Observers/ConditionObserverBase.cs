using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public interface IConditionObserver : LamosInteractive.Goap.Interfaces.IConditionObserver
    {
        void SetWorldData(IWorldData worldData);
        bool IsMet(IEffect effect);
    }
    
    public abstract class ConditionObserverBase<TCondition, TEffect> : IConditionObserver
        where TCondition : ICondition
        where TEffect : IEffect
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData worldData)
        {
            this.WorldData = worldData;
        }
        
        public bool IsMet(ICondition condition)
        {
            return this.IsMet((TCondition) condition);
        }
        
        public bool IsMet(IEffect effect)
        {
            return this.IsMet((TEffect) effect);
        }

        protected abstract bool IsMet(TCondition condition);
        protected abstract bool IsMet(TEffect effect);
    }
}