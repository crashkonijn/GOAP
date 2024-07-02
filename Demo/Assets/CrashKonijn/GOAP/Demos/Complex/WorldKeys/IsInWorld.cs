using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;

namespace CrashKonijn.Goap.Demos.Complex.WorldKeys
{
    public class IsInWorld<THoldable> : WorldKeyBase
        where THoldable : IHoldable
    {
    }
}