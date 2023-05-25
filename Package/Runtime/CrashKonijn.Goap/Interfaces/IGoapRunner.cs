using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapRunner
    {
        void Register(IGoapSet goapSet);
        Graph GetGraph(IGoapSet goapSet);
        bool Knows(IGoapSet goapSet);
        IMonoAgent[] Agents { get; }

        [System.Obsolete("'Sets' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        IGoapSet[] Sets { get; }

        IGoapSet[] GoapSets { get; }

        [System.Obsolete("'GetSet' is deprecated, please use 'GetGoapSet' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        IGoapSet GetSet(string id);

        IGoapSet GetGoapSet(string id);
    }
}