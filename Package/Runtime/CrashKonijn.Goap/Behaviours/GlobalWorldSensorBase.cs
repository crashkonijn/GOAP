using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class GlobalWorldSensorBase : IGlobalWorldSensor
    {
        private WorldSensorConfig config;
        public WorldKey Key => this.config.key;
        
        public void SetConfig(WorldSensorConfig config) => this.config = config;

        public abstract bool Sense();
    }
}