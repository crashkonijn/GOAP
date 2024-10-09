using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class GlobalWorldData : WorldDataBase, IGlobalWorldData
    {
        protected override bool IsLocal => false;

        public override (bool Exists, int Value) GetWorldValue(Type worldKey)
        {
            if (!this.States.ContainsKey(worldKey))
                return (false, 0);

            return (true, this.States[worldKey].Value);
        }

        public override ITarget GetTargetValue(Type targetKey)
        {
            if (!this.Targets.ContainsKey(targetKey))
                return null;

            return this.Targets[targetKey].Value;
        }

        public override IWorldDataState<ITarget> GetTargetState(Type targetKey)
        {
            if (!this.Targets.ContainsKey(targetKey))
                return null;

            return this.Targets[targetKey];
        }

        public override IWorldDataState<int> GetWorldState(Type worldKey)
        {
            if (!this.States.ContainsKey(worldKey))
                return null;

            return this.States[worldKey];
        }
    }
}
