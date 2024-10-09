using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.Runtime
{
    public class Condition : ICondition
    {
        public IWorldKey WorldKey { get; set; }
        public Comparison Comparison { get; set; }
        public int Amount { get; set; }

        public Condition()
        {
        }

        public Condition(IWorldKey worldKey, Comparison comparison, int amount)
        {
            this.WorldKey = worldKey;
            this.Comparison = comparison;
            this.Amount = amount;
        }
    }
}