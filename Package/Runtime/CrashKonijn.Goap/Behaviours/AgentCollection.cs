using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentCollection : IAgentCollection
    {
        private HashSet<IMonoAgent> agents = new();
        private HashSet<IMonoAgent> queue = new HashSet<IMonoAgent>();

        public HashSet<IMonoAgent> All() => this.agents;
        
        public void Add(IMonoAgent agent)
        {
            if (this.agents.Contains(agent))
                return;
            
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
        
        public int GetQueueCount() => this.queue.Count;

        public IMonoAgent[] GetQueue()
        {
            var data = this.queue.ToArray();
            
            this.queue.Clear();

            return data;
        }
    }
}