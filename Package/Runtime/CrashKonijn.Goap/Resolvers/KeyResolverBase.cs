using System;
using CrashKonijn.Goap.Interfaces;
using IAction = CrashKonijn.Goap.Resolver.Interfaces.IAction;

namespace CrashKonijn.Goap.Resolvers
{
    public abstract class KeyResolverBase : IKeyResolver
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData globalWorldData)
        {
            this.WorldData = globalWorldData;
        }
        
        public string GetKey(IAction action, Resolver.Interfaces.ICondition condition)
        {
            if (action is IActionBase tAction)
                return this.GetKey(tAction, (ICondition) condition);
            if (action is IGoalBase tGoal)
                return this.GetKey(tGoal, (ICondition) condition);

            throw new Exception($"Unsupported type {action.GetType()}");
        }

        public string GetKey(IAction action, Resolver.Interfaces.IEffect effect)
        {
            return this.GetKey((IActionBase) action, (IEffect) effect);
        }

        protected abstract string GetKey(IActionBase action, ICondition key);
        protected abstract string GetKey(IActionBase action, IEffect key);
        protected abstract string GetKey(IGoalBase goal, ICondition key);
    }
}