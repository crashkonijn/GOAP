using System;
using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [DefaultExecutionOrder(-99)]
    [Obsolete("Use AgentTypeBehaviour instead!")]
    [RequireComponent(typeof(AgentTypeBehaviour))]
    public class GoapSetBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GoapSetConfigScriptable config;

        [SerializeField]
        private GoapBehaviour runner;

        public IAgentType AgentType { get; private set; }

        private void Awake()
        {
            var agentType = new AgentTypeFactory(GoapConfig.Default).Create(this.config);

            this.runner.Register(agentType);

            this.AgentType = agentType;
        }
    }
}
