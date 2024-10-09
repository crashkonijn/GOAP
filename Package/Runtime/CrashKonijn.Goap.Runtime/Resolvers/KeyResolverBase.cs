using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class KeyResolverBase : IKeyResolver
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData globalWorldData)
        {
            this.WorldData = globalWorldData;
        }

        public string GetKey(IConnectable action, ICondition condition)
        {
            if (action is IAction tAction)
                return this.GetKey(tAction, (ICondition) condition);
            if (action is IGoal tGoal)
                return this.GetKey(tGoal, (ICondition) condition);

            throw new Exception($"Unsupported type {action.GetType()}");
        }

        public string GetKey(IConnectable action, IEffect effect)
        {
            return this.GetKey((IAction) action, (IEffect) effect);
        }

        protected abstract string GetKey(IAction action, ICondition key);
        protected abstract string GetKey(IAction action, IEffect key);
        protected abstract string GetKey(IGoal goal, ICondition key);
    }
}
