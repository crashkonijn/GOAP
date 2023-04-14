using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;

namespace CrashKonijn.Goap.Classes
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