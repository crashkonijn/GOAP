using System.Collections.Generic;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentCollection : MonoBehaviour
    {
        private Dictionary<GoapSet, HashSet<Agent>> agents = new();

        public Dictionary<GoapSet, HashSet<Agent>> All() => this.agents;
        
        public void Add(Agent agent)
        {
            this.GetSet(agent).Add(agent);
        }

        public void Remove(Agent agent)
        {
            this.GetSet(agent).Remove(agent);
        }

        private HashSet<Agent> GetSet(Agent agent)
        {
            if (this.agents.TryGetValue(agent.goapSet, out var set))
                return set;

            set = new HashSet<Agent>();
            this.agents.Add(agent.goapSet, set);
            return set;
        }
    }
}