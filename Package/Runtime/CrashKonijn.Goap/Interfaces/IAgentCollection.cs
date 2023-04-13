using System.Collections.Generic;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IAgentCollection
    {
        HashSet<IMonoAgent> All();
        void Add(IMonoAgent agent);
        void Remove(IMonoAgent agent);
        void Enqueue(IMonoAgent agent);
        IMonoAgent[] GetQueue();
        int GetQueueCount();
    }
}