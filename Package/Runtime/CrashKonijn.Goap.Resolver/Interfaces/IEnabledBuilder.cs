using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IEnabledBuilder
    {
        IEnabledBuilder SetEnabled(IConnectable action, bool executable);
        void Clear();
        bool[] Build();
    }
}