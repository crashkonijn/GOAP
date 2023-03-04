using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public class ConditionObserver : ConditionObserverBase
    {
        public override bool IsMet(ICondition condition)
        {
            if (condition.Positive)
                return this.WorldData.IsTrue(condition.WorldKey);
            
            return !this.WorldData.IsTrue(condition.WorldKey);
        }

        public override bool IsMet(IEffect effect)
        {
            if (effect.Positive)
                return this.WorldData.IsTrue(effect.WorldKey);
            
            return !this.WorldData.IsTrue(effect.WorldKey);
        }
    }
}