using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class ValueCondition : IValueCondition
    {
        public IWorldKey WorldKey { get; set; }
        public Comparison Comparison { get; set; }
        public int Amount { get; set; }

        public ValueCondition()
        {
        }

        public ValueCondition(IWorldKey worldKey, Comparison comparison, int amount)
        {
            this.WorldKey = worldKey;
            this.Comparison = comparison;
            this.Amount = amount;
        }
    }
}