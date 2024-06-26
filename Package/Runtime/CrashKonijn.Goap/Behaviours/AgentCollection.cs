using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentCollection : IAgentCollection
    {
        private readonly IAgentType agentType;
        private HashSet<IMonoGoapAgent> agents = new();
        private HashSet<IMonoGoapAgent> queue = new HashSet<IMonoGoapAgent>();

        public AgentCollection(IAgentType agentType)
        {
            this.agentType = agentType;
        }

        public HashSet<IMonoGoapAgent> All() => this.agents;
        
        public void Add(IMonoGoapAgent agent)
        {
            if (this.agents.Contains(agent))
                return;
            
            this.agents.Add(agent );
            this.agentType.Events.AgentRegistered(agent);
        }

        public void Remove(IMonoGoapAgent agent)
        {
            if (!this.agents.Contains(agent))
                return;
            
            this.agents.Remove(agent);
            this.agentType.Events.AgentUnregistered(agent);
        }

        public void Enqueue(IMonoGoapAgent agent)
        {
            this.queue.Add(agent);
        }
        
        public int GetQueueCount() => this.queue.Count;

        public IMonoGoapAgent[] GetQueue()
        {
            var data = this.queue.ToArray();
            
            this.queue.Clear();

            return data;
        }
    }
}