using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/GoapSetConfig")]
    public class GoapSetConfig : ScriptableObject
    {
        public List<ActionBase> actions;
        public List<GoalConfig> goals;

        public List<TargetSensorConfig> targetSensors;
        public List<WorldSensorConfig> worldSensors;
    }
}