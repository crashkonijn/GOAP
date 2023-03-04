using System.Linq;
using CrashKonijn.Goap.Interfaces;

namespace CrashKonijn.Goap.Observers
{
    public class CostObserver : CostObserverBase
    {
        public override float GetCost(IActionBase current, IActionBase[] path)
        {
            var cost = current.GetCost(this.WorldData);
            var last = path.LastOrDefault();

            if (last == null)
                return cost;

            if (last.Config.Target == null)
                return cost;
            
            return cost + current.GetDistanceCost(this.WorldData.GetTarget(current), this.WorldData.GetTarget(last));
        }
    }
}