using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class MultiSensorConfig : IMultiSensorConfig
    {
        public string Name { get; set; }
        public string ClassType { get; set; }
    }
}