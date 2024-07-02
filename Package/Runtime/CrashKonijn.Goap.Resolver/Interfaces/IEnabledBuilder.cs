using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public interface IEnabledBuilder
    {
        IEnabledBuilder SetEnabled(IConnectable action, bool executable);
        void Clear();
        bool[] Build();
    }
}