using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    [DefaultExecutionOrder(-99)]
    public class AgentTypeBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected AgentTypeScriptable config;

        [SerializeField]
        protected GoapBehaviour runner;

        public IAgentType AgentType { get; private set; }
        public AgentTypeScriptable Config => this.config;

        protected virtual void Awake()
        {
            var config = this.config.Create();

            var agentType = new AgentTypeFactory(this.runner.Config).Create(config);

            this.runner.Register(agentType);

            this.AgentType = agentType;
        }
    }
}
