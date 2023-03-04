using System.Linq;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public abstract class CostObserverBase : ICostObserver
    {
        protected IWorldData WorldData { get; private set; }

        public void SetWorldData(IWorldData worldData)
        {
            this.WorldData = worldData;
        }
        
        public float GetCost(Resolver.Interfaces.IAction current, Resolver.Interfaces.IAction[] path)
        {
            return this.GetCost((IActionBase) current, path.Where(x => x is IActionBase).Cast<IActionBase>().ToArray());
        }
        
        public abstract float GetCost(IActionBase current, IActionBase[] path);
    }
}