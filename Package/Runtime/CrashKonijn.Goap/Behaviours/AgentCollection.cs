using System.Collections.Generic;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public interface IAgentCollection
    {
        HashSet<IMonoAgent> All();
        void Add(IMonoAgent agent);
        void Remove(IMonoAgent agent);
    }

    public class AgentCollection : IAgentCollection
    {
        private HashSet<IMonoAgent> agents = new();

        public HashSet<IMonoAgent> All() => this.agents;
        
        public void Add(IMonoAgent agent)
        {
            this.agents.Add(agent);
        }

        public void Remove(IMonoAgent agent)
        {
            this.agents.Remove(agent);
        }
    }
}