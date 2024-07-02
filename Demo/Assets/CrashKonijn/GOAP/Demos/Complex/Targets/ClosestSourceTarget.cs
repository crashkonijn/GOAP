using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.Targets
{
    public class ClosestSourceTarget<TGatherable> : TargetKeyBase
        where TGatherable : IGatherable
    {
    }
}