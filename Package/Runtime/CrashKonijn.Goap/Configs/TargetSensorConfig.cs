using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class TargetSensorConfig<TSensor> : ITargetSensorConfig
        where TSensor : ITargetSensor
    {
        public TargetSensorConfig()
        {
            this.Name = typeof(TSensor).Name;
            this.ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
        
        public TargetSensorConfig(string name)
        {
            this.Name = name;
            this.ClassType = typeof(TSensor).AssemblyQualifiedName;
        }
        
        public string Name { get; }
        public string ClassType { get; }
        public ITargetKey Key { get; set; }
    }
}