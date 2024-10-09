using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class WorldSensorConfig<TSensor> : WorldSensorConfig
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
    }

    public class WorldSensorConfig : IWorldSensorConfig, IClassCallbackConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
        public IWorldKey Key { get; set; }
        public Action<object> Callback { get; set; }
    }
}
