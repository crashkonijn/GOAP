using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class LocalTargetSensorBase : ILocalTargetSensor
    {
        public ITargetKey key => this.Config.Key;
        public ITargetSensorConfig Config { get; private set; }
        public void SetConfig(ITargetSensorConfig config) => this.Config = config;
        
        public abstract ITarget Sense(IMonoAgent agent);
    }
}