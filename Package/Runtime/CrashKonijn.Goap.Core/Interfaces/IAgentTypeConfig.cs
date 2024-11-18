using System.Collections.Generic;

namespace CrashKonijn.Goap.Core
{
    public interface IAgentTypeConfig : IConfig
    {
        public List<IActionConfig> Actions { get; }
        public List<IGoalConfig> Goals { get; }

        public List<ITargetSensorConfig> TargetSensors { get; }
        public List<IWorldSensorConfig> WorldSensors { get; }
        public List<IMultiSensorConfig> MultiSensors { get; }
    }

    public interface ICapabilityConfig : IAgentTypeConfig { }
}
