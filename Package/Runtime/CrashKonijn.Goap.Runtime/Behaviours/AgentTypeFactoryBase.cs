using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class AgentTypeFactoryBase : MonoBehaviour, IAgentTypeFactory
    {
        private IGoapConfig config;

        public void Construct(IGoapConfig config)
        {
            this.config = config;
        }

        public abstract IAgentTypeConfig Create();

        protected AgentTypeBuilder CreateBuilder(string name)
        {
            return new AgentTypeBuilder(this.config?.GoapInjector, name);
        }
    }
}