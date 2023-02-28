using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Models;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapRunner : IGoapRunner
    {
        private Dictionary<IGoapSet, GoapSetJobRunner> sets = new();

        public void Register(IGoapSet set) => this.sets.Add(set, new GoapSetJobRunner(set));

        public void Run()
        {
            foreach (var runner in this.sets.Values)
            {
                runner.Run();
            }
        }

        public void Complete()
        {
            foreach (var runner in this.sets.Values)
            {
                runner.Complete();
            }
        }

        public void Dispose()
        {
            foreach (var runner in this.sets.Values)
            {
                runner.Dispose();
            }
        }

        public Graph GetGraph(IGoapSet set) => this.sets[set].GetGraph();
        public bool Knows(IGoapSet set) => this.sets.ContainsKey(set);
    }
}