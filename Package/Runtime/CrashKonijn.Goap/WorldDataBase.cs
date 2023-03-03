using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap
{
    public abstract class WorldDataBase : IWorldData
    {
        public Dictionary<IWorldKey, WorldKeyState> States { get; } = new();
        public Dictionary<ITargetKey, ITarget> Targets { get; } = new();

        public ITarget GetTarget(IActionBase action)
        {
            if (!this.Targets.ContainsKey(action.Config.target))
                return null;
            
            return this.Targets[action.Config.target];
        }

        public bool IsTrue(IWorldKey worldKey)
        {
            if (!this.States.ContainsKey(worldKey))
                return false;
            
            return this.States[worldKey] == WorldKeyState.True;
        }

        public void SetState(IWorldKey key, WorldKeyState state)
        {
            if (this.States.ContainsKey(key))
            {
                this.States[key] = state;
                return;
            }
            
            this.States.Add(key, state);
        }
        
        public void SetTarget(ITargetKey key, ITarget target)
        {
            if (this.Targets.ContainsKey(key))
            {
                this.Targets[key] = target;
                return;
            }
            
            this.Targets.Add(key, target);
        }
    }
}