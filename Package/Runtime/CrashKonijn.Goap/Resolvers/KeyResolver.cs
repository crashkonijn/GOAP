using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Interfaces;

namespace CrashKonijn.Goap.Resolvers
{
    public interface IKeyResolver
    {
        string GetKey(IAction action, ICondition condition);
        string GetKey(IAction action, IEffect effect);
        void SetWorldData(IWorldData globalWorldData);
    }

    public class KeyResolver : ActionKeyResolverBase<IActionBase, IGoalBase>, IKeyResolver
    {
        protected override string GetKey(IActionBase action, Condition condition)
        {
            return condition.worldKey.name + this.GetText(condition.positive);
        }

        protected override string GetKey(IActionBase action, Effect effect)
        {
            return effect.worldKey.name + this.GetText(effect.positive);
        }

        protected override string GetKey(IGoalBase action, Condition condition)
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