using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap
{
    public abstract class WorldDataBase : IWorldData
    {
        public Dictionary<IWorldKey, int> States { get; } = new();
        public Dictionary<ITargetKey, ITarget> Targets { get; } = new();

        public ITarget GetTarget(IActionBase action)
        {
            if (action.Config.Target == null)
                return null;
            
            if (!this.Targets.ContainsKey(action.Config.Target))
                return null;
            
            return this.Targets[action.Config.Target];
        }

        public bool IsTrue(IWorldKey worldKey, Comparison comparison, int value)
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
            if (key == null)
                return;
            
            if (this.Targets.ContainsKey(key))
            {
                this.Targets[key] = target;
                return;
            }
            
            this.Targets.Add(key, target);
        }
    }
}