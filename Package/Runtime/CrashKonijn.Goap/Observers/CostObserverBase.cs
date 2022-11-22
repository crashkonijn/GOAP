using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;
using LamosInteractive.Goap.Interfaces;
using ICostObserver = CrashKonijn.Goap.Interfaces.ICostObserver;

namespace CrashKonijn.Goap.Observers
{
    public abstract class CostObserverBase<TAction> : ICostObserver
        where TAction : IAction
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData worldData)
        {
            this.WorldData = worldData;
        }
        
        public float GetCost(IAction current, List<IAction> path)
        {
            return this.GetCost((TAction)current, path.Where(x => x is TAction).Cast<TAction>().ToList());
        }

        protected abstract float GetCost(TAction current, List<TAction> path);
    }
}