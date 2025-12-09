using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public interface IWorldKeyBuilder
    {
        IWorldKey GetKey<TKey>()
            where TKey : IWorldKey;
    }
}