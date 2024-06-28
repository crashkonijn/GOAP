using System.Collections.Generic;

namespace CrashKonijn.Goap.Core
{
    public interface IAgentCollection
    {
        HashSet<IMonoGoapActionProvider> All();
        void Add(IMonoGoapActionProvider actionProvider);
        void Remove(IMonoGoapActionProvider actionProvider);
        void Enqueue(IMonoGoapActionProvider actionProvider);
        IMonoGoapActionProvider[] GetQueue();
        int GetQueueCount();
    }
}