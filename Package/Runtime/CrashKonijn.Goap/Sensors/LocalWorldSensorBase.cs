using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Sensors
{
    public abstract class LocalWorldSensorBase : ILocalWorldSensor
    {
        private WorldSensorConfig config;
        public WorldKey Key => this.config.key;
        
        public void SetConfig(WorldSensorConfig config) => this.config = config;
        
        public abstract bool Sense(IMonoAgent agent);
    }
}