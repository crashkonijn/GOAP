using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IWorldData
    {
        public Dictionary<IWorldKey, int> States { get; }
        public Dictionary<ITargetKey, ITarget> Targets { get; }
        public ITarget GetTarget(IActionBase action);
        void SetState(IWorldKey key, int state);
        void SetTarget(ITargetKey key, ITarget target);
        public bool IsTrue(IWorldKey worldKey, Comparison comparison, int value);
    }
}