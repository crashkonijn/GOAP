using CrashKonijn.Agent.Core;

namespace CrashKonijn.Agent.Runtime
{
    public class AgentTimers : IAgentTimers
    {
        public ITimer Action { get; } = new Timer();
        public ITimer Goal { get; } = new Timer();
        public ITimer Resolve { get; } = new Timer();
    }
}