using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public interface IExecutableBuilder
    {
        IExecutableBuilder SetExecutable(IConnectable action, bool executable);
        void Clear();
        bool[] Build();
    }
}