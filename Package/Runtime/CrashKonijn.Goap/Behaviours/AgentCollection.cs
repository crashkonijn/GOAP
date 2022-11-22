using System.Collections.Generic;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public interface IAgentCollection
    {
        Dictionary<IGoapSet, HashSet<IMonoAgent>> All();
        void Add(IMonoAgent agent);
        void Remove(IMonoAgent agent);
    }

    public class AgentCollection : MonoBehaviour, IAgentCollection
    {
        private Dictionary<IGoapSet, HashSet<IMonoAgent>> agents = new();

        public Dictionary<IGoapSet, HashSet<IMonoAgent>> All() => this.agents;
        
        public void Add(IMonoAgent agent)
        {
            this.GetSet(agent).Add(agent);
        }

        public void Remove(IMonoAgent agent)
        {
            this.GetSet(agent).Remove(agent);
        }

        private HashSet<IMonoAgent> GetSet(IMonoAgent agent)
        {
            if (this.agents.TryGetValue(agent.GoapSet, out var set))
                return set;

            set = new HashSet<IMonoAgent>();
            this.agents.Add(agent.GoapSet, set);
            return set;
        }
    }
}