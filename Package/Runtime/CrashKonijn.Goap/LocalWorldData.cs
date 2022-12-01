using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap
{
    public class LocalWorldData : IWorldData
    {
        public HashSet<IWorldKey> States { get; set; }
        public Dictionary<ITargetKey, ITarget> Targets { get; set; }

        public ITarget GetTarget(IActionBase action)
        {
            if (action?.Config?.target == null)
            {
                return null;
            }

            this.Targets.TryGetValue(action.Config.target, out var value);
            
            return value;
        }

        public LocalWorldData(IWorldData globalWorldData)
        {
            this.States = new HashSet<IWorldKey>(globalWorldData.States);
            this.Targets = new Dictionary<ITargetKey, ITarget>(globalWorldData.Targets);
        }
    }
}