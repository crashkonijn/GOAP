using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/AgentTypeConfig")]
    public class AgentTypeScriptable : ScriptableObject, IAgentTypeConfig
    {
        [ActionDebuggerClass]
        public string debuggerClass;
        
        public List<BehaviourConfigScriptable> behaviours = new();

        public string Name => this.name;
        public List<IActionConfig> Actions => new();
        public List<IGoalConfig> Goals => new();
        public List<ITargetSensorConfig> TargetSensors => new();
        public List<IWorldSensorConfig> WorldSensors => new();
        public string DebuggerClass => this.debuggerClass;
    }
}