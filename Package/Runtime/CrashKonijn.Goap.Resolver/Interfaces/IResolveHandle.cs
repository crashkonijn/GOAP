using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Resolver
{
    public interface IResolveHandle
    {
        IConnectable[] Complete();
    }
}