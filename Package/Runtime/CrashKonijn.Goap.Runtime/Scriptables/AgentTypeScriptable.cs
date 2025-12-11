using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Runtime
{
    [CreateAssetMenu(menuName = "Goap/AgentTypeConfig")]
    public class AgentTypeScriptable : ScriptableObject
    {
        [FormerlySerializedAs("capabilityFactories")]
        public List<ScriptableCapabilityFactoryBase> capabilities = new();

        public string Name => this.name;

        public IAgentTypeConfig Create()
        {
            // Aggregate lists
            var allGoals = new List<IGoalConfig>();
            var allActions = new List<IActionConfig>();
            var allWorldSensors = new List<IWorldSensorConfig>();
            var allTargetSensors = new List<ITargetSensorConfig>();
            var allMultiSensors = new List<IMultiSensorConfig>();

            for (int i = 0; i < this.capabilities.Count; i++)
            {
                var capabilityConfig = this.capabilities[i].Create();

                if (capabilityConfig.Goals != null)
                    allGoals.AddRange(capabilityConfig.Goals);

                if (capabilityConfig.Actions != null)
                    allActions.AddRange(capabilityConfig.Actions);

                if (capabilityConfig.WorldSensors != null)
                    allWorldSensors.AddRange(capabilityConfig.WorldSensors);

                if (capabilityConfig.TargetSensors != null)
                    allTargetSensors.AddRange(capabilityConfig.TargetSensors);

                if (capabilityConfig.MultiSensors != null)
                    allMultiSensors.AddRange(capabilityConfig.MultiSensors);
            }

            return new AgentTypeConfig(this.name)
            {
                Goals = allGoals,
                Actions = allActions,
                WorldSensors = allWorldSensors,
                TargetSensors = allTargetSensors,
                MultiSensors = allMultiSensors,
            };
        }
    }
}
