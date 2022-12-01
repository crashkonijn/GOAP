using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Serializables;

namespace CrashKonijn.Goap.Observers
{
    public class ConditionObserver : ConditionObserverBase<ICondition, IEffect>
    {
        protected override bool IsMet(ICondition condition)
        {
            if (condition.Positive)
                return this.WorldData.States.Contains(condition.WorldKey);
            
            return !this.WorldData.States.Contains(condition.WorldKey);
        }

        protected override bool IsMet(IEffect effect)
        {
            if (effect.Positive)
                return this.WorldData.States.Contains(effect.WorldKey);
            
            return !this.WorldData.States.Contains(effect.WorldKey);
        }
    }
}