namespace CrashKonijn.Goap.Core
{
    public interface ICondition
    {
        public IWorldKey WorldKey { get; }
        public Comparison Comparison { get; }
        public int Amount { get; }
    }
}
