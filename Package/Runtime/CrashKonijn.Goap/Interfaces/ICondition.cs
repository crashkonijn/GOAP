using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Interfaces
{
    public interface ICondition : Resolver.Interfaces.ICondition {
        public IWorldKey WorldKey { get; }
        public Comparison Comparison { get; }
        public int Amount { get; }
    }
}