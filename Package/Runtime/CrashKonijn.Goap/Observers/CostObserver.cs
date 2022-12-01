using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public class CostObserver : CostObserverBase<IActionBase>
    {
        protected override float GetCost(IActionBase current, List<IActionBase> path)
        {
            var cost = current.GetCost(this.WorldData);
            var last = path.LastOrDefault();

            if (last == null)
                return cost;

            if (last.Config.target == null)
                return cost;
            
            return cost + current.GetDistanceCost(this.WorldData.GetTarget(current), this.WorldData.GetTarget(last));
        }
    }
}