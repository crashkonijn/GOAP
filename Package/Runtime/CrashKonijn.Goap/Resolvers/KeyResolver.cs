using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Serializables;

namespace CrashKonijn.Goap.Resolvers
{
    public class KeyResolver : KeyResolverBase<IActionBase, IGoalBase>
    {
        protected override string GetKey(IActionBase action, ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.Positive);
        }

        protected override string GetKey(IActionBase action, IEffect effect)
        {
            return effect.WorldKey.Name + this.GetText(effect.Positive);
        }

        protected override string GetKey(IGoalBase action, ICondition condition)
        {
            return condition.WorldKey.Name + this.GetText(condition.Positive);
        }

        private string GetText(bool value)
        {
            if (value)
                return "_true";

            return "_false";
        }
    }
}