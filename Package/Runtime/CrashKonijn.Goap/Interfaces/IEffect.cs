using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface IEffect : Resolver.Interfaces.IEffect
    {
        public IWorldKey WorldKey { get; }
        public bool Positive { get; }
    }
}