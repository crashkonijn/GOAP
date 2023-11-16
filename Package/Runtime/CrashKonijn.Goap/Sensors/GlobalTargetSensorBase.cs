using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class GlobalTargetSensorBase : IGlobalTargetSensor
    {
        public ITargetKey Key => this.Config.Key;
        
        public ITargetSensorConfig Config { get; private set; }
        public void SetConfig(ITargetSensorConfig config) => this.Config = config;

        public abstract void Created();

        public void Sense(IWorldData worldData)
        {
            worldData.SetTarget(this.Key, this.Sense());
        }
        
        public abstract ITarget Sense();
    }
}