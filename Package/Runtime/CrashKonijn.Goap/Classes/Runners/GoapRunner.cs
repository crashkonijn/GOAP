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
        private Dictionary<IGoapSet, GoapSetJobRunner> sets = new();
        private Stopwatch stopwatch = new();

        public float RunTime { get; private set; }
        public float CompleteTime { get; private set; }

        public void Register(IGoapSet set) => this.sets.Add(set, new GoapSetJobRunner(set, new GraphResolver(set.GetAllNodes().ToArray(), set.GoapConfig.KeyResolver)));

        public void Run()
        {
            this.stopwatch.Restart();
            
            foreach (var runner in this.sets.Values)
            {
                runner.Run();
            }
            
            this.RunTime = this.GetElapsedMs();
                        
            foreach (var agent in this.Agents)
            {
                agent.Run();
            }
        }

        public void Complete()
        {
            this.stopwatch.Restart();
            
            foreach (var runner in this.sets.Values)
            {
                runner.Complete();
            }
            
            this.CompleteTime = this.GetElapsedMs();
        }

        public void Dispose()
        {
            foreach (var runner in this.sets.Values)
            {
                runner.Dispose();
            }
        }

        private float GetElapsedMs()
        {
            this.stopwatch.Stop();
            
            return (float) ((double)this.stopwatch.ElapsedTicks / Stopwatch.Frequency * 1000);
        }

        public Graph GetGraph(IGoapSet set) => this.sets[set].GetGraph();
        public bool Knows(IGoapSet set) => this.sets.ContainsKey(set);
        public IMonoAgent[] Agents => this.sets.Keys.SelectMany(x => x.Agents.All()).ToArray();
        public IGoapSet[] Sets => this.sets.Keys.ToArray();

        public IGoapSet GetSet(string id)
        {
            var set = this.Sets.FirstOrDefault(x => x.Id == id);

            if (set == null)
                throw new KeyNotFoundException($"No set with id {id} found");
            
            return set;
        }

        public int QueueCount => this.sets.Keys.Sum(x => x.Agents.GetQueueCount());
    }
}