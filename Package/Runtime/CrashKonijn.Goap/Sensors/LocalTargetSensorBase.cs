using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class LocalTargetSensorBase : ILocalTargetSensor
    {
        private TargetSensorConfig config;
        public TargetKey Key => this.config.key;
        public void SetConfig(TargetSensorConfig config) => this.config = config;
        
        public abstract ITarget Sense(IMonoAgent agent);
    }
}