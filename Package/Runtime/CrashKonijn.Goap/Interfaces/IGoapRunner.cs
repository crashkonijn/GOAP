using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapRunner
    {
        void Register(IAgentType agentType);
        Graph GetGraph(IAgentType agentType);
        bool Knows(IAgentType agentType);
        IMonoAgent[] Agents { get; }

        [System.Obsolete("'Sets' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        IAgentType[] Sets { get; }

        IAgentType[] GoapSets { get; }

        [System.Obsolete("'GetSet' is deprecated, please use 'GetGoapSet' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        IAgentType GetSet(string id);

        IAgentType GetGoapSet(string id);
    }
}