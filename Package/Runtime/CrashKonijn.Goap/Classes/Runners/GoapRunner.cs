using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Classes.Runners
{
    public class GoapRunner : IGoapRunner
    {
        private readonly GoapConfig config;
        private HashSet<IGoapSet> sets = new();
        private GoapSetRunner goapSetRunner;

        public GoapRunner(GoapConfig config)
        {
            this.config = config;
            this.goapSetRunner = new GoapSetRunner();
        }

        public void Register(IGoapSet set) => this.sets.Add(set);

        public void Run()
        {
            foreach (var goapSet in this.sets)
            {
                this.goapSetRunner.Run(goapSet);
            }
        }
    }
}