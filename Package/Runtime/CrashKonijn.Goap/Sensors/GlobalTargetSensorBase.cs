using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class GlobalTargetSensorBase : IGlobalTargetSensor
    {
        public ITargetKey key => this.Config.Key;
        
        public ITargetSensorConfig Config { get; private set; }
        public void SetConfig(ITargetSensorConfig config) => this.Config = config;

        public abstract ITarget Sense();
    }
}