using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class AgentTypeConfig : IAgentTypeConfig
    {
        public AgentTypeConfig(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public List<IActionConfig> Actions { get; set; }
        public List<IGoalConfig> Goals { get; set; }
        public List<ITargetSensorConfig> TargetSensors { get; set; }
        public List<IWorldSensorConfig> WorldSensors { get; set; }
        public List<IMultiSensorConfig> MultiSensors { get; set; }
    }

    public class CapabilityConfig : ICapabilityConfig
    {
        public CapabilityConfig(string name)
        {
            this.Name = name;
        }

        public string Name { get; }

        public List<IActionConfig> Actions { get; set; }
        public List<IGoalConfig> Goals { get; set; }
        public List<ITargetSensorConfig> TargetSensors { get; set; }
        public List<IWorldSensorConfig> WorldSensors { get; set; }
        public List<IMultiSensorConfig> MultiSensors { get; set; }
    }
}
