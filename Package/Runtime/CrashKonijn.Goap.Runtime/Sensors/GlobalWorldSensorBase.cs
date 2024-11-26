using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class GlobalWorldSensorBase : IGlobalWorldSensor
    {
        public IWorldKey Key => this.Config.Key;
        public virtual ISensorTimer Timer => SensorTimer.Always;
        public IWorldSensorConfig Config { get; private set; }
        public void SetConfig(IWorldSensorConfig config) => this.Config = config;

        /// <summary>
        ///     Called when the sensor is created.
        /// </summary>
        public abstract void Created();

        public Type[] GetKeys() => new[] { this.Key.GetType() };

        /// <summary>
        ///     Senses the world data using this sensor. Don't override this method.
        /// </summary>
        /// <param name="data">The world data.</param>
        public void Sense(IWorldData data)
        {
            var state = data.GetWorldState(this.Key.GetType());

            if (!this.Timer.ShouldSense(state?.Timer))
                return;

            data.SetState(this.Key, this.Sense());
        }

        /// <summary>
        ///     Senses the world data.
        /// </summary>
        /// <returns>The sensed value.</returns>
        public abstract SenseValue Sense();
    }
}
