using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using CrashKonijn.Goap.Resolvers;

namespace CrashKonijn.Goap.Classes
{
    public class GoapConfig
    {
        public CostObserverBase<IActionBase> CostObserver { get; }
        public ConditionObserverBase<Condition, Effect> ConditionObserver { get; }
        public ActionKeyResolverBase<IActionBase, IGoalBase> KeyResolver { get; }
        
        public GoapConfig(CostObserverBase<IActionBase> costObserver, ConditionObserverBase<Condition, Effect> conditionObserver, ActionKeyResolverBase<IActionBase, IGoalBase> keyResolver)
        {
            this.CostObserver = costObserver;
            this.ConditionObserver = conditionObserver;
            this.KeyResolver = keyResolver;
        }
    }
}