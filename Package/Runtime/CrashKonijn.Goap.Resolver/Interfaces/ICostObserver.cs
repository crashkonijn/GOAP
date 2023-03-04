using System.Collections.Generic;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface ICostObserver
    {
        float GetCost(IAction current, IAction[] path);
    }
}
