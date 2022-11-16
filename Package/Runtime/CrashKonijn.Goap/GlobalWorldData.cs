using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap
{
    public class GlobalWorldData : IWorldData
    {
        public HashSet<WorldKey> States { get; set; }
        public Dictionary<TargetKey, ITarget> Targets { get; set; }

        public ITarget GetTarget(IActionBase action)
        {
            return this.Targets[action.Config.target];
        }
    }
}