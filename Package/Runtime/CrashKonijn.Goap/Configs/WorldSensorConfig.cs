using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class WorldSensorConfig<TSensor> : IWorldSensorConfig
        where TSensor : IWorldSensor
    {
        public WorldSensorConfig()
        {
            this.Name = typeof(TSensor).Name;
            this.ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
        
        public WorldSensorConfig(string name)
        {
            this.Name = name;
            this.ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
        
        public string Name { get; }
        public string ClassType { get; set; }
        public IWorldKey Key { get; set; }
    }
}