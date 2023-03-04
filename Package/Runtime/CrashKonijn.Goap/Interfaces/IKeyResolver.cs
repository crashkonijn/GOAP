
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IKeyResolver : IActionKeyResolver
    {
        void SetWorldData(IWorldData globalWorldData);
    }
}