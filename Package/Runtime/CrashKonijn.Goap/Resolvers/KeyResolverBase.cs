using System;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Interfaces;
using ICondition = CrashKonijn.Goap.Classes.ICondition;
using IEffect = CrashKonijn.Goap.Classes.IEffect;
using ILamosAction = LamosInteractive.Goap.Interfaces.IAction;
using ILamosCondition = LamosInteractive.Goap.Interfaces.ICondition;
using ILamosEffect = LamosInteractive.Goap.Interfaces.IEffect;

namespace CrashKonijn.Goap.Resolvers
{
    public abstract class KeyResolverBase<TAction, TGoal> : IActionKeyResolver
        where TAction : IAction
        where TGoal : IGoalBase
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData globalWorldData)
        {
            this.WorldData = globalWorldData;
        }
        
        public string GetKey(ILamosAction action, ILamosCondition condition)
        {
            if (action is TAction tAction)
                return this.GetKey(tAction, (ICondition) condition);
            if (action is TGoal tGoal)
                return this.GetKey(tGoal, (ICondition) condition);

            throw new Exception($"Unsupported type {action.GetType()}");
        }

        public string GetKey(ILamosAction action, ILamosEffect effect)
        {
            return this.GetKey((TAction) action, (IEffect) effect);
        }

        protected abstract string GetKey(TAction action, ICondition key);
        protected abstract string GetKey(TAction action, IEffect key);
        protected abstract string GetKey(TGoal goal, ICondition key);
    }
}