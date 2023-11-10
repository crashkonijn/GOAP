using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IExecutableBuilder
    {
        IExecutableBuilder SetExecutable(IConnectable action, bool executable);
        void Clear();
        bool[] Build();
    }
}