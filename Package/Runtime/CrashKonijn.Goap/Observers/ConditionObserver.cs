using CrashKonijn.Goap.Classes;

namespace CrashKonijn.Goap.Observers
{
    public class ConditionObserver : ConditionObserverBase<Condition, Effect>
    {
        protected override bool IsMet(Condition condition)
        {
            if (condition.positive)
                return this.WorldData.States.Contains(condition.worldKey);
            
            return !this.WorldData.States.Contains(condition.worldKey);
        }

        protected override bool IsMet(Effect effect)
        {
            if (effect.positive)
                return this.WorldData.States.Contains(effect.worldKey);
            
            return !this.WorldData.States.Contains(effect.worldKey);
        }
    }
}