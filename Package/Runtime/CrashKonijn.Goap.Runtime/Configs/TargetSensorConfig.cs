using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class TargetSensorConfig<TSensor> : TargetSensorConfig
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
    }

    public class TargetSensorConfig : ITargetSensorConfig, IClassCallbackConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
        public ITargetKey Key { get; set; }
        public Action<object> Callback { get; set; }
    }
}
