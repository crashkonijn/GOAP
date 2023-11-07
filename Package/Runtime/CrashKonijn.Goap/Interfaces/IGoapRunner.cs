using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapRunner
    {
        void Register(IAgentType agentType);
        Graph GetGraph(IAgentType agentType);
        bool Knows(IAgentType agentType);
        IMonoAgent[] Agents { get; }

        IAgentType[] AgentTypes { get; }

        IAgentType GetAgentType(string id);
    }
}