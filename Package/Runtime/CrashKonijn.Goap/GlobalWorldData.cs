using System;
using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap
{
    public class GlobalWorldData : WorldDataBase, IGlobalWorldData
    {
        public override (bool Exists, int Value) GetWorldValue(Type worldKey)
        {
            if (!this.States.ContainsKey(worldKey))
                return (false, 0);
            
            return (true, this.States[worldKey]);
        }

        public override ITarget GetTargetValue(Type targetKey)
        {
            if (!this.Targets.ContainsKey(targetKey))
                return null;
            
            return this.Targets[targetKey];
        }
    }
}