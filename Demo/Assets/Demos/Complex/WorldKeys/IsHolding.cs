using CrashKonijn.Goap.Behaviours;
using Demos.Complex.Interfaces;

namespace Demos.Complex.WorldKeys
{
    public class IsHolding<THoldable> : WorldKeyBase
        where THoldable : IHoldable
    {
    }
}