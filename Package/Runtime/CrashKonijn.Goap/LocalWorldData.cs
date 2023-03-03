using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap
{
    public class LocalWorldData : IWorldData
    {
        public HashSet<IWorldKey> States { get; }
        public Dictionary<ITargetKey, ITarget> Targets { get; }

        public ITarget GetTarget(IActionBase action)
        {
            if (action?.Config?.target == null)
            {
                return null;
            }

            this.Targets.TryGetValue(action.Config.target, out var value);
            
            return value;
        }

        public void AddStates(IEnumerable<IWorldKey> states)
        {
            foreach (var worldKey in states)
            {
                this.States.Add(worldKey);
            }
        }

        public void AddTargets(Dictionary<ITargetKey, ITarget> targets)
        {
            foreach (var (key, value) in targets)
            {
                this.Targets.Add(key, value);
            }
        }

        public LocalWorldData(IWorldData globalWorldData)
        {
            this.States = new HashSet<IWorldKey>(globalWorldData.States);
            this.Targets = new Dictionary<ITargetKey, ITarget>(globalWorldData.Targets);
        }
    }
}