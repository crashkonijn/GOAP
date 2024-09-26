using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class LocalWorldData : WorldDataBase, ILocalWorldData
    {
        protected override bool IsLocal => true;
        public IGlobalWorldData GlobalData { get; private set; }

        public void SetParent(IGlobalWorldData globalData)
        {
            this.GlobalData = globalData;
        }

        public override (bool Exists, int Value) GetWorldValue(Type worldKey)
        {
            if (this.States.TryGetValue(worldKey, out var state))
                return (true, state.Value);

            return this.GlobalData.GetWorldValue(worldKey);
        }

        public override ITarget GetTargetValue(Type targetKey)
        {
            if (this.Targets.TryGetValue(targetKey, out var value))
                return value.Value;

            return this.GlobalData.GetTargetValue(targetKey);
        }

        public override IWorldDataState<ITarget> GetTargetState(Type targetKey)
        {
            if (this.Targets.TryGetValue(targetKey, out var value))
                return value;

            return this.GlobalData.GetTargetState(targetKey);
        }

        public override IWorldDataState<int> GetWorldState(Type worldKey)
        {
            if (this.States.TryGetValue(worldKey, out var state))
                return state;

            return this.GlobalData.GetWorldState(worldKey);
        }
    }
}
