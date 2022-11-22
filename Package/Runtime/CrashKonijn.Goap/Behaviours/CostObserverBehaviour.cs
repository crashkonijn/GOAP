using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using LamosInteractive.Goap.Interfaces;
using UnityEngine;
using ICostObserver = CrashKonijn.Goap.Interfaces.ICostObserver;

namespace CrashKonijn.Goap.Behaviours
{
    public class CostObserverBehaviour : MonoBehaviour, ICostObserver
    {
        private readonly CostObserver costObserver = new();

        public float GetCost(IAction current, List<IAction> path) => this.costObserver.GetCost(current, path);

        public void SetWorldData(IWorldData worldData) => this.costObserver.SetWorldData(worldData);
    }
}