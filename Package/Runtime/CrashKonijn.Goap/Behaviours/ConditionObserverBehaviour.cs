using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using LamosInteractive.Goap.Interfaces;
using UnityEngine;
using IConditionObserver = CrashKonijn.Goap.Observers.IConditionObserver;

namespace CrashKonijn.Goap.Behaviours
{
    public class ConditionObserverBehaviour : MonoBehaviour, IConditionObserver
    {
        private readonly ConditionObserver observer = new();

        public bool IsMet(ICondition condition) => this.observer.IsMet(condition);
        public bool IsMet(IEffect effect) => this.observer.IsMet(effect);
        public void SetWorldData(IWorldData worldData) => this.observer.SetWorldData(worldData);
    }
}