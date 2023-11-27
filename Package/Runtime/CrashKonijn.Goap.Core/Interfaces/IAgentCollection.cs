using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentCollection
    {
        HashSet<IMonoAgent> All();
        void Add(IMonoAgent agent);
        void Remove(IMonoAgent agent);
    }
}