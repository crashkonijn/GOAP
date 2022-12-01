using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Resolvers
{
    public class KeyResolver : KeyResolverBase<IActionBase, IGoalBase>
    {
        protected override string GetKey(IActionBase action, Condition condition)
        {
            return condition.worldKey.Name + this.GetText(condition.positive);
        }

        protected override string GetKey(IActionBase action, Effect effect)
        {
            return effect.worldKey.Name + this.GetText(effect.positive);
        }

        protected override string GetKey(IGoalBase action, Condition condition)
        {
            return condition.worldKey.Name + this.GetText(condition.positive);
        }

        private string GetText(bool value)
        {
            if (value)
                return "_true";

            return "_false";
        }
    }
}