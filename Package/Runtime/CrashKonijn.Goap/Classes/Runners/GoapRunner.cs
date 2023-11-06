using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapRunner : IGoapRunner
    {
        private Dictionary<IAgentType, GoapSetJobRunner> goapSets = new();
        private Stopwatch stopwatch = new();

        public float RunTime { get; private set; }
        public float CompleteTime { get; private set; }

        public void Register(IAgentType agentType) => this.goapSets.Add(agentType, new GoapSetJobRunner(agentType, new GraphResolver(agentType.GetAllNodes().ToArray(), agentType.GoapConfig.KeyResolver)));

        public void Run()
        {
            this.stopwatch.Restart();
            
            foreach (var runner in this.goapSets.Values)
            {
                runner.Run();
            }
            
            this.RunTime = this.GetElapsedMs();
                        
            foreach (var agent in this.Agents)
            {
                if (agent.IsNull())
                    continue;
                
                agent.Run();
            }
        }

        public void Complete()
        {
            this.stopwatch.Restart();
            
            foreach (var runner in this.goapSets.Values)
            {
                runner.Complete();
            }
            
            this.CompleteTime = this.GetElapsedMs();
        }

        public void Dispose()
        {
            foreach (var runner in this.goapSets.Values)
            {
                runner.Dispose();
            }
        }

        private float GetElapsedMs()
        {
            this.stopwatch.Stop();
            
            return (float) ((double)this.stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
        }

        public Graph GetGraph(IAgentType agentType) => this.goapSets[agentType].GetGraph();
        public bool Knows(IAgentType agentType) => this.goapSets.ContainsKey(agentType);
        public IMonoAgent[] Agents => this.goapSets.Keys.SelectMany(x => x.Agents.All()).ToArray();
        
        [System.Obsolete("'Sets' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IAgentType[] Sets => this.goapSets.Keys.ToArray();

        public IAgentType[] GoapSets => this.goapSets.Keys.ToArray();

        [System.Obsolete("'GetSet' is deprecated, please use 'GetGoapSet' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IAgentType GetSet(string id)
        {
            var set = this.Sets.FirstOrDefault(x => x.Id == id);

            if (set == null)
                throw new KeyNotFoundException($"No set with id {id} found");
            
            return set;
        }

        public IAgentType GetGoapSet(string id)
        {
            var goapSet = this.GoapSets.FirstOrDefault(x => x.Id == id);

            if (goapSet == null)
                throw new KeyNotFoundException($"No goapSet with id {id} found");

            return goapSet;
        }

        public int QueueCount => this.goapSets.Keys.Sum(x => x.Agents.GetQueueCount());
    }
}