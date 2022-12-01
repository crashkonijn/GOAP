using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoapSetConfig")]
    public class GoapSetConfigScriptable : ScriptableObject, IGoapSetConfig
    {
        public List<ActionBase> actions;
        public List<GoalConfigScriptable> goals;

        public List<TargetSensorConfigScriptable> targetSensors;
        public List<WorldSensorConfigScriptable> worldSensors;

        public string Name => this.name;
        public List<ActionBase> Actions => this.actions;
        public List<IGoalConfig> Goals => this.goals.Cast<IGoalConfig>().ToList();
        public List<ITargetSensorConfig> TargetSensors => this.targetSensors.Cast<ITargetSensorConfig>().ToList();
        public List<IWorldSensorConfig> WorldSensors => this.worldSensors.Cast<IWorldSensorConfig>().ToList();
    }
}