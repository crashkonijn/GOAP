using System.Collections.Generic;

namespace LamosInteractive.Goap.Interfaces
{
    public interface IGraphResolver
    {
        (IAction Action, List<IAction> Path) Resolve(IAction rootAction);
    }
}