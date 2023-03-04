using CrashKonijn.Goap.Configs.Interfaces;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ICondition : Resolver.Interfaces.ICondition {
        public IWorldKey WorldKey { get; }
        public bool Positive { get; }
    }
}