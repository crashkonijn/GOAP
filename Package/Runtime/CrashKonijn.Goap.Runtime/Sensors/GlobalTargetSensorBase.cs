using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class GlobalTargetSensorBase : IGlobalTargetSensor
    {
        private Type key;
        public ITargetKey Key => this.Config.Key;
        public virtual ISensorTimer Timer => SensorTimer.Always;
        public ITargetSensorConfig Config { get; private set; }
        public void SetConfig(ITargetSensorConfig config) => this.Config = config;
        public Type[] GetKeys() => new[] { this.Key.GetType() };

        public abstract void Created();

        public void Sense(IWorldData worldData)
        {
            var state = worldData.GetTargetState(this.Key.GetType());

            if (!this.Timer.ShouldSense(state?.Timer))
                return;

            worldData.SetTarget(this.Key, this.Sense(state?.Value));
        }

        public abstract ITarget Sense(ITarget target);
    }
}
