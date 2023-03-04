using System;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IAction
    {
        Guid Guid { get; }
        IEffect[] Effects { get; }
        ICondition[] Conditions { get; }
    }
}