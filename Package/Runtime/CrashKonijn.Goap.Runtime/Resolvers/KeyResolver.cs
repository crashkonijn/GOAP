using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class KeyResolver : KeyResolverBase
    {
        public override string GetKey(ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.Comparison);
        }

        public override string GetKey(IEffect effect)
        {
            return effect.WorldKey.Name + this.GetText(effect.Increase);
        }

        public override bool AreConflicting(IEffect effect, ICondition condition)
        {
            if (effect.WorldKey.Name != condition.WorldKey.Name)
                return false;

            if (this.GetText(effect.Increase) == this.GetText(condition.Comparison))
                return false;

            return true;
        }

        private string GetText(bool value)
        {
            if (value)
                return "_increase";

            return "_decrease";
        }

        private string GetText(Comparison comparison)
        {
            switch (comparison)
            {
                case Comparison.GreaterThan:
                case Comparison.GreaterThanOrEqual:
                    return "_increase";
                case Comparison.SmallerThan:
                case Comparison.SmallerThanOrEqual:
                    return "_decrease";
            }

            throw new Exception($"Comparison type {comparison} not supported");
        }
    }
}
