using System;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Interfaces;
using UnityEngine;
using IAction = LamosInteractive.Goap.Interfaces.IAction;

namespace CrashKonijn.Goap.Resolvers
{
    public abstract class ActionKeyResolverBase<TAction, TGoal> : IActionKeyResolver
        where TAction : IAction
        where TGoal : IGoalBase
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData globalWorldData)
        {
            this.WorldData = globalWorldData;
        }
        
        public string GetKey(IAction action, ICondition condition)
        {
            if (action is TAction tAction)
                return this.GetKey(tAction, (Condition) condition);
            if (action is TGoal tGoal)
                return this.GetKey(tGoal, (Condition) condition);

            throw new Exception($"Unsupported type {action.GetType()}");
        }

        public string GetKey(IAction action, IEffect effect)
        {
            return this.GetKey((TAction) action, (Effect) effect);
        }

        protected abstract string GetKey(TAction action, Condition key);
        protected abstract string GetKey(TAction action, Effect key);
        protected abstract string GetKey(TGoal action, Condition key);
    }
}