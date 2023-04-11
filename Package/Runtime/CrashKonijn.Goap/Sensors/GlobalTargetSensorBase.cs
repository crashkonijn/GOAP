using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class GlobalTargetSensorBase : IGlobalTargetSensor
    {
        public ITargetKey Key => this.Config.Key;
        
        public ITargetSensorConfig Config { get; private set; }
        public void SetConfig(ITargetSensorConfig config) => this.Config = config;

        public abstract void Created();
        public abstract ITarget Sense();
    }
}