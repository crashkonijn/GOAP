using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IGoapRunner
    {
        void Register(IGoapSet set);
        Graph GetGraph(IGoapSet set);
        bool Knows(IGoapSet set);
        IMonoAgent[] Agents { get; }
    }
}