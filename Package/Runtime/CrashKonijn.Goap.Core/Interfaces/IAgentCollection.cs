using System.Collections.Generic;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IAgentCollection
    {
        HashSet<IMonoGoapAgent> All();
        void Add(IMonoGoapAgent agent);
        void Remove(IMonoGoapAgent agent);
        void Enqueue(IMonoGoapAgent agent);
        IMonoGoapAgent[] GetQueue();
        int GetQueueCount();
    }
}