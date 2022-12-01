using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap
{
    public class GlobalWorldData : IWorldData
    {
        public HashSet<IWorldKey> States { get; set; }
        public Dictionary<ITargetKey, ITarget> Targets { get; set; }

        public ITarget GetTarget(IActionBase action)
        {
            return this.Targets[action.Config.target];
        }
    }
}