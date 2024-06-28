using System;

namespace CrashKonijn.Goap.Core
{
    public interface IConnectable
    {
        Guid Guid { get; }
        IEffect[] Effects { get; }
        ICondition[] Conditions { get; }
    }
}