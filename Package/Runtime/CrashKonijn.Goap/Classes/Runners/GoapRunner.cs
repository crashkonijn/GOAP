using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapRunner : IGoapRunner
    {
        private readonly IAgentCollection agentCollection;
        private readonly GoapConfig config;
        private HashSet<GoapSet> sets = new();

        public GoapRunner(IAgentCollection agentCollection, GoapConfig config)
        {
            this.agentCollection = agentCollection;
            this.config = config;
        }

        public void Register(GoapSet set) => this.sets.Add(set);
        public void Register(IMonoAgent agent) => this.agentCollection.Add(agent);
        public void Unregister(IMonoAgent agent) => this.agentCollection.Remove(agent);

        public void Initialize()
        {
            foreach (var set in this.sets)
            {
                set.Initialize(this.config);
            }
        }

        public void Run()
        {
            foreach (var (set, agents) in this.agentCollection.All())
            {
                set.Run(agents);
            }
        }
    }
}