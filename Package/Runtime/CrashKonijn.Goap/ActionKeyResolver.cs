using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap
{
    public class ActionKeyResolver : ActionKeyResolverBase<IActionBase, IGoalBase>
    {
        public override string GetKey(IActionBase action, Condition condition)
        {
            return condition.worldKey.name + this.GetText(condition.positive);
        }

        public override string GetKey(IActionBase action, Effect effect)
        {
            return effect.worldKey.name + this.GetText(effect.positive);
        }

        public override string GetKey(IGoalBase action, Condition condition)
        {
            return condition.worldKey.name + this.GetText(condition.positive);
        }

        private string GetText(bool value)
        {
            if (value)
                return "_true";

            return "_false";
        }
    }
}