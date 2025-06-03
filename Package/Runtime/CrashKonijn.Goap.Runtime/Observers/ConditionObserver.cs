using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ConditionObserver : ConditionObserverBase
    {
        public override bool IsMet(ICondition condition)
        {
            if (condition is IValueCondition valueCondition)
                return IsMet(valueCondition);
            
            if (condition is IReferenceCondition referenceCondition)
                return IsMet(referenceCondition);
            
            throw new ArgumentException($"Unknown condition type: {condition.GetType()}");
        }

        private bool IsMet(IValueCondition condition)
        {
            return this.WorldData.IsTrue(condition.WorldKey, condition.Comparison, condition.Amount);
        }
        
        private bool IsMet(IReferenceCondition condition)
        {
            return this.WorldData.IsTrue(condition.WorldKey, condition.Comparison, condition.ValueKey);
        }
    }
}