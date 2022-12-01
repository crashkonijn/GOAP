using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Configs
{
    public interface IGoapSetConfig : IConfig
    {
        public List<ActionBase> Actions { get; }
        public List<IGoalConfig> Goals { get; }

        public List<ITargetSensorConfig> TargetSensors { get; }
        public List<IWorldSensorConfig> WorldSensors { get; }
    }
    
    public class GoapSetConfig : IGoapSetConfig
    {
        public GoapSetConfig(string name)
        {
            this.Name = name;
        }
        
        public string Name { get; }

        public List<ActionBase> Actions { get; set; }
        public List<IGoalConfig> Goals { get; set; }
        public List<ITargetSensorConfig> TargetSensors { get; set; }
        public List<IWorldSensorConfig> WorldSensors { get; set; }
    }
}