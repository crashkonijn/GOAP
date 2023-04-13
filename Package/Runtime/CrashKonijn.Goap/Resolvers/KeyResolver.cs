using System;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Resolvers
{
    public class KeyResolver : KeyResolverBase
    {
        protected override string GetKey(IActionBase action, ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.Comparison);
        }

        protected override string GetKey(IActionBase action, IEffect effect)
        {
            return effect.WorldKey.Name + this.GetText(effect.Increase);
        }

        protected override string GetKey(IGoalBase action, ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.Comparison);
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