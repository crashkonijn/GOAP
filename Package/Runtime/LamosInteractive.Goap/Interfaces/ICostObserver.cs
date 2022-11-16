using System.Collections.Generic;

namespace LamosInteractive.Goap.Interfaces
{
    public interface ICostObserver
    {
        float GetCost(IAction current, List<IAction> path);
    }
}
