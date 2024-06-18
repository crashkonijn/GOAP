using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver.Models;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IGraphResolver
    {
        IResolveHandle StartResolve(RunData runData);
        IEnabledBuilder GetEnabledBuilder();
        IExecutableBuilder GetExecutableBuilder();
        IPositionBuilder GetPositionBuilder();
        ICostBuilder GetCostBuilder();
        IGraph GetGraph();
        int GetIndex(IConnectable action);
        IAction GetAction(int index);
        void Dispose();
        IConditionBuilder GetConditionBuilder();
    }
}