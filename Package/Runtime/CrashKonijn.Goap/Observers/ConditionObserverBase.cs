using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Observers
{
    public abstract class ConditionObserverBase<TCondition, TEffect> : MonoBehaviour, IConditionObserver
        where TCondition : ICondition
        where TEffect : IEffect
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData globalWorldData)
        {
            this.WorldData = globalWorldData;
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