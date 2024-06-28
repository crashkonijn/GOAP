using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class LocalWorldData : WorldDataBase, ILocalWorldData
    {
        public IGlobalWorldData GlobalData { get; private set; }
        
        public void SetParent(IGlobalWorldData globalData)
        {
            this.GlobalData = globalData;
        }

        public override (bool Exists, int Value) GetWorldValue(Type worldKey)
        {
            if (this.States.TryGetValue(worldKey, out var state))
                return (true, state);
            
            return this.GlobalData.GetWorldValue(worldKey);
        }

        public override ITarget GetTargetValue(Type targetKey)
        {
            if (this.Targets.TryGetValue(targetKey, out var value))
                return value;
            
            return this.GlobalData.GetTargetValue(targetKey);
        }
    }
}