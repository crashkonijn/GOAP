using System.Collections.Generic;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IWorldData
    {
        public HashSet<WorldKey> States { get; }
        public Dictionary<TargetKey, ITarget> Targets { get; }

        public ITarget GetTarget(IActionBase action);
    }
}