using CrashKonijn.Goap.Core.Enums;

namespace CrashKonijn.Goap.Core.Interfaces
{
    public interface ICondition {
        public IWorldKey WorldKey { get; }
        public Comparison Comparison { get; }
        public int Amount { get; }
    }
}