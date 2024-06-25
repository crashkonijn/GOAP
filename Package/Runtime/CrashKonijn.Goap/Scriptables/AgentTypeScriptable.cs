using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Scriptables
{
    [CreateAssetMenu(menuName = "Goap/AgentTypeConfig")]
    public class AgentTypeScriptable : ScriptableObject
    {
        [FormerlySerializedAs("capabilityFactories")]
        public List<ScriptableCapabilityFactoryBase> capabilities = new();

        public string Name => this.name;

        public IAgentTypeConfig Create()
        {
            var configs = this.capabilities
                .Select(behaviour => behaviour.Create())
                .ToList();
            
            return new AgentTypeConfig(this.name)
            {
                Goals = configs.SelectMany(x => x.Goals).ToList(),
                Actions = configs.SelectMany(x => x.Actions).ToList(),
                WorldSensors = configs.SelectMany(x => x.WorldSensors).ToList(),
                TargetSensors = configs.SelectMany(x => x.TargetSensors).ToList(),
                MultiSensors = configs.SelectMany(x => x.MultiSensors).ToList(),
            };
        }
    }
}