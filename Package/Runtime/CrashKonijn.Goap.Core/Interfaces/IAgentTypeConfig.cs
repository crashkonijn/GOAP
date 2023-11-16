﻿using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentTypeConfig : IConfig
    {
        public List<IActionConfig> Actions { get; }
        public List<IGoalConfig> Goals { get; }

        public List<ITargetSensorConfig> TargetSensors { get; }
        public List<IWorldSensorConfig> WorldSensors { get; }
        public List<IMultiSensorConfig> MultiSensors { get; set; }
        public string DebuggerClass { get; }
    }
}