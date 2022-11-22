using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using CrashKonijn.Goap.Resolvers;

namespace CrashKonijn.Goap.Classes
{
    public class GoapConfig
    {
        public ICostObserver CostObserver { get; }
        public IConditionObserver ConditionObserver { get; }
        public IKeyResolver KeyResolver { get; }
        
        public GoapConfig(ICostObserver costObserver, IConditionObserver conditionObserver, IKeyResolver keyResolver)
        {
            this.CostObserver = costObserver;
            this.ConditionObserver = conditionObserver;
            this.KeyResolver = keyResolver;
        }
    }
}