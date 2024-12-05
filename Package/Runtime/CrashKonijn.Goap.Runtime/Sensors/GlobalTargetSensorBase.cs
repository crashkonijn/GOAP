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

        /// <summary>
        ///     Called when the sensor is created.
        /// </summary>
        public abstract void Created();

        /// <summary>
        ///     Senses the world data using this sensor. Do not override this method.
        /// </summary>
        /// <param name="worldData">The world data.</param>
        public void Sense(IWorldData worldData)
        {
            var state = worldData.GetTargetState(this.Key.GetType());

            if (!this.Timer.ShouldSense(state?.Timer))
                return;

            worldData.SetTarget(this.Key, this.Sense(state?.Value));
        }

        /// <summary>
        ///     Senses the world data using the specified existing target.
        /// </summary>
        /// <param name="target">The existing target. (The previously returned instance by this sensor).</param>
        /// <returns>The sensed target.</returns>
        public abstract ITarget Sense(ITarget target);
    }
}
