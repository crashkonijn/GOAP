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
        private Dictionary<IGoapSet, GoapSetJobRunner> goapSets = new();
        private Stopwatch stopwatch = new();

        public float RunTime { get; private set; }
        public float CompleteTime { get; private set; }

        public void Register(IGoapSet goapSet) => this.goapSets.Add(goapSet, new GoapSetJobRunner(goapSet, new GraphResolver(goapSet.GetAllNodes().ToArray(), goapSet.GoapConfig.KeyResolver)));

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

        public Graph GetGraph(IGoapSet goapSet) => this.goapSets[goapSet].GetGraph();
        public bool Knows(IGoapSet goapSet) => this.goapSets.ContainsKey(goapSet);
        public IMonoAgent[] Agents => this.goapSets.Keys.SelectMany(x => x.Agents.All()).ToArray();
        
        [System.Obsolete("'Sets' is deprecated, please use 'GoapSets' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IGoapSet[] Sets => this.goapSets.Keys.ToArray();

        public IGoapSet[] GoapSets => this.goapSets.Keys.ToArray();

        [System.Obsolete("'GetSet' is deprecated, please use 'GetGoapSet' instead.   Exact same functionality, name changed to mitigate confusion with the word 'set' which could have many meanings.")]
        public IGoapSet GetSet(string id)
        {
            var set = this.Sets.FirstOrDefault(x => x.Id == id);

            if (set == null)
                throw new KeyNotFoundException($"No set with id {id} found");
            
            return set;
        }

        public IGoapSet GetGoapSet(string id)
        {
            var goapSet = this.GoapSets.FirstOrDefault(x => x.Id == id);

            if (goapSet == null)
                throw new KeyNotFoundException($"No goapSet with id {id} found");

            return goapSet;
        }

        public int QueueCount => this.goapSets.Keys.Sum(x => x.Agents.GetQueueCount());
    }
}