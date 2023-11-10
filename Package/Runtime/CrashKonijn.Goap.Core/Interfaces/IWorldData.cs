using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IWorldData
    {
        public Dictionary<IWorldKey, int> States { get; }
        public Dictionary<ITargetKey, ITarget> Targets { get; }
        public ITarget GetTarget(IAction action);
        void SetState(IWorldKey key, int state);
        void SetTarget(ITargetKey key, ITarget target);
        public bool IsTrue(IWorldKey worldKey, Comparison comparison, int value);
        public void Apply(IWorldData worldData);
    }
}