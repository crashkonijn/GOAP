using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap
{
    public abstract class WorldDataBase : IWorldData
    {
        public Dictionary<Type, int> States { get; } = new();
        public Dictionary<Type, ITarget> Targets { get; } = new();

        public ITarget GetTarget(IAction action)
        {
            if (action.Config.Target == null)
                return null;
            
            if (!this.Targets.ContainsKey(action.Config.Target.GetType()))
                return null;
            
            return this.Targets[action.Config.Target.GetType()];
        }

        public bool IsTrue<TWorldKey>(Comparison comparison, int value)
        {
            return this.IsTrue(typeof(TWorldKey), comparison, value);
        }

        public bool IsTrue(IWorldKey worldKey, Comparison comparison, int value)
        {
            return this.IsTrue(worldKey.GetType(), comparison, value);
        }
        
        public bool IsTrue(Type worldKey, Comparison comparison, int value)
        {
            if (!this.States.ContainsKey(worldKey))
                return false;
            
            var state = this.States[worldKey];

            switch (comparison)
            {
                case Comparison.GreaterThan:
                    return state > value;
                case Comparison.GreaterThanOrEqual:
                    return state >= value;
                case Comparison.SmallerThan:
                    return state < value;
                case Comparison.SmallerThanOrEqual:
                    return state <= value;
            }

            return false;
        }

        public void SetState(IWorldKey key, int state)
        {
            this.SetState(key.GetType(), state);
        }

        public void SetState<TKey>(int state) where TKey : IWorldKey
        {
            this.SetState(typeof(TKey), state);
        }

        public void SetState(Type key, int state)
        {
            if (key == null)
                return;

            if (this.States.ContainsKey(key))
            {
                this.States[key] = state;
                return;
            }
            
            this.States.Add(key, state);
        }
        
        public void SetTarget(ITargetKey key, ITarget target)
        {
            this.SetTarget(key.GetType(), target);
        }

        public void SetTarget<TKey>(ITarget target) where TKey : ITargetKey
        {
            this.SetTarget(typeof(TKey), target);
        }
        
        public void SetTarget(Type key, ITarget target)
        {
            if (key == null)
                return;

            if (this.Targets.ContainsKey(key))
            {
                this.Targets[key] = target;
                return;
            }
            
            this.Targets.Add(key, target);
        }
        
        public void Apply(IWorldData worldData)
        {
            foreach (var (key, value) in worldData.States)
            {
                this.SetState(key, value);
            }
            
            foreach (var (key, value) in worldData.Targets)
            {
                this.SetTarget(key, value);
            }
        }
    }
}