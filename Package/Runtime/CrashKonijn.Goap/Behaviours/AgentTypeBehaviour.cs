using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Scriptables;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    [DefaultExecutionOrder(-99)]
    public class AgentTypeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AgentTypeScriptable config;

        [SerializeField]
        private GoapRunnerBehaviour runner;

        public IAgentType AgentType { get; private set; }

        private void Awake()
        {
            var agentType = new AgentTypeFactory(GoapConfig.Default).Create(this.config);

            this.runner.Register(agentType);
            
            this.AgentType = agentType;
        }
    }
}