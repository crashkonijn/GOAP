using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IGoap
    {
        IGoapEvents Events { get; }
        Dictionary<IAgentType, IAgentTypeJobRunner> AgentTypeRunners { get; }
        IGoapController Controller { get; }
        void Register(IAgentType agentType);
        IGraph GetGraph(IAgentType agentType);
        bool Knows(IAgentType agentType);
        List<IMonoGoapAgent> Agents { get; }

        IAgentType[] AgentTypes { get; }

        IAgentType GetAgentType(string id);
    }
}