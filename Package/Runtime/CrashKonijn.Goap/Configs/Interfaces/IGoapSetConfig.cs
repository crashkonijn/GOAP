using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Configs.Interfaces
{
    public interface IGoapSetConfig : IConfig
    {
        public List<IActionConfig> Actions { get; }
        public List<IGoalConfig> Goals { get; }

        public List<ITargetSensorConfig> TargetSensors { get; }
        public List<IWorldSensorConfig> WorldSensors { get; }
        public string DebuggerClass { get; }
    }
}