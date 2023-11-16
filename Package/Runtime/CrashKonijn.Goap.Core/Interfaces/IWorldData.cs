using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface IWorldData
    {
        public Dictionary<Type, int> States { get; }
        public Dictionary<Type, ITarget> Targets { get; }
        public ITarget GetTarget(IAction action);
        void SetState(IWorldKey key, int state);
        void SetState<TKey>(int state) where TKey : IWorldKey;
        void SetTarget(ITargetKey key, ITarget target);
        void SetTarget<TKey>(ITarget target) where TKey : ITargetKey;
        public bool IsTrue(IWorldKey worldKey, Comparison comparison, int value);
        public void Apply(IWorldData worldData);
    }
}