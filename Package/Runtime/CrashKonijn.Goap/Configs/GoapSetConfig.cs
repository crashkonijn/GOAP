using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public class GoapSetConfig : IGoapSetConfig
    {
        public GoapSetConfig(string name)
        {
            this.Name = name;
        }
        
        public string Name { get; }

        public List<IActionConfig> Actions { get; set; }
        public List<IGoalConfig> Goals { get; set; }
        public List<ITargetSensorConfig> TargetSensors { get; set; }
        public List<IWorldSensorConfig> WorldSensors { get; set; }
        public string DebuggerClass { get; set; }
    }
}