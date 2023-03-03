using System.Collections.Generic;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IWorldData
    {
        public Dictionary<IWorldKey, WorldKeyState> States { get; }
        public Dictionary<ITargetKey, ITarget> Targets { get; }
        public ITarget GetTarget(IActionBase action);
        void SetState(IWorldKey key, WorldKeyState state);
        void SetTarget(ITargetKey key, ITarget target);
        public bool IsTrue(IWorldKey worldKey);
    }
}