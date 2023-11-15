namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoapRunner
    {
        void Register(IAgentType agentType);
        IGraph GetGraph(IAgentType agentType);
        bool Knows(IAgentType agentType);
        IMonoAgent[] Agents { get; }

        IAgentType[] AgentTypes { get; }

        IAgentType GetAgentType(string id);
    }
}