using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public interface IAgentCollection
    {
        HashSet<IMonoAgent> All();
        void Add(IMonoAgent agent);
        void Remove(IMonoAgent agent);
        void Enqueue(IMonoAgent agent);
        IMonoAgent[] GetQueue();
    }

    public class AgentCollection : IAgentCollection
    {
        private HashSet<IMonoAgent> agents = new();
        private HashSet<IMonoAgent> queue = new HashSet<IMonoAgent>();

        public HashSet<IMonoAgent> All() => this.agents;
        
        public void Add(IMonoAgent agent)
        {
            this.agents.Add(agent);
        }

        public void Remove(IMonoAgent agent)
        {
            this.agents.Remove(agent);
        }

        public void Enqueue(IMonoAgent agent)
        {
            this.queue.Add(agent);
        }

        public IMonoAgent[] GetQueue()
        {
            var data = this.queue.ToArray();
            
            this.queue.Clear();

            return data;
        }
    }
}