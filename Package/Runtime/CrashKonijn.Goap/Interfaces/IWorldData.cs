using System.Collections.Generic;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Scriptables;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IWorldData
    {
        public HashSet<IWorldKey> States { get; }
        public Dictionary<ITargetKey, ITarget> Targets { get; }
        public ITarget GetTarget(IActionBase action);
    }
}