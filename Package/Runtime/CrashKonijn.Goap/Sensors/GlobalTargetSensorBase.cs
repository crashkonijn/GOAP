using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class GlobalTargetSensorBase : IGlobalTargetSensor
    {
        private TargetSensorConfig config;
        public TargetKey Key => this.config.key;
        public void SetConfig(TargetSensorConfig config) => this.config = config;

        public abstract ITarget Sense();
    }
}