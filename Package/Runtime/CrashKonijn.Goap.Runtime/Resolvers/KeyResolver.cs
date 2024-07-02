using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class KeyResolver : KeyResolverBase
    {
        protected override string GetKey(IAction action, ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.Comparison);
        }

        protected override string GetKey(IAction action, IEffect effect)
        {
            return effect.WorldKey.Name + this.GetText(effect.Increase);
        }

        protected override string GetKey(IGoal action, ICondition condition)
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