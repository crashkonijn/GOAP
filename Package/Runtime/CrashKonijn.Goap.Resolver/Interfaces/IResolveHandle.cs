using CrashKonijn.Goap.Core.Interfaces;

namespace CrashKonijn.Goap.Resolver.Interfaces
{
    public interface IResolveHandle
    {
        IConnectable[] Complete();
    }
}