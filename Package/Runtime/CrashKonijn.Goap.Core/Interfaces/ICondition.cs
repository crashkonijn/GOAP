namespace CrashKonijn.Goap.Core
{
    public interface ICondition
    {
        public IWorldKey WorldKey { get; }
        public Comparison Comparison { get; }
    }
    
    public interface IValueCondition : ICondition
    {
        public int Amount { get; }
    }
    
    public interface IReferenceCondition : ICondition
    {
        public IWorldKey ValueKey { get; }
    }
}
