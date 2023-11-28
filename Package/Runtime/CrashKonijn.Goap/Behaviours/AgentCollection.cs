using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentCollection : IAgentCollection
    {
        private readonly IAgentType agentType;
        private HashSet<IMonoAgent> agents = new();
        private HashSet<IMonoAgent> queue = new HashSet<IMonoAgent>();

        public AgentCollection(IAgentType agentType)
        {
            this.agentType = agentType;
        }

        public HashSet<IMonoAgent> All() => this.agents;
        
        public void Add(IMonoAgent agent)
        {
            if (this.agents.Contains(agent))
                return;
            
            this.agents.Add(agent);
            this.agentType.Events.AgentRegistered(agent);
        }

        public void Remove(IMonoAgent agent)
        {
            if (!this.agents.Contains(agent))
                return;
            
            this.agents.Remove(agent);
            this.agentType.Events.AgentUnregistered(agent);
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