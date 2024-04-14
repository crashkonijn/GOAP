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
        [ActionDebuggerClass] public string debuggerClass;

        public List<CapabilityConfigScriptable> behaviours = new();

        public string Name => this.name;

        public List<IActionConfig> Actions
        {
            get
            {
                var generator = this.GetGenerator();

                return this.behaviours
                    .SelectMany(behaviour => behaviour.GetActions(generator))
                    .ToList();
            }
        }

        public List<IGoalConfig> Goals
        {
            get
            {
                var generator = this.GetGenerator();

                return this.behaviours
                    .SelectMany(behaviour => behaviour.GetGoals(generator))
                    .ToList();
            }
        }

        public List<ITargetSensorConfig> TargetSensors
        {
            get
            {
                var generator = this.GetGenerator();

                return this.behaviours
                    .SelectMany(behaviour => behaviour.GetTargetSensors(generator))
                    .ToList();
            }
        }

        public List<IWorldSensorConfig> WorldSensors
        {
            get
            {
                var generator = this.GetGenerator();

                return this.behaviours
                    .SelectMany(behaviour => behaviour.GetWorldSensors(generator))
                    .ToList();
            }
        }

        public List<IMultiSensorConfig> MultiSensors
        {
            get
            {
                var generator = this.GetGenerator();
                
                return this.behaviours
                    .SelectMany(behaviour => behaviour.GetMultiSensors(generator))
                    .ToList();
            }
        }

        public string DebuggerClass => this.debuggerClass;
    }
}