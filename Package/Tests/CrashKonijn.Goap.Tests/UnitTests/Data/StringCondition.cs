using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class StringCondition : ICondition
    {
        private readonly string key;

        public StringCondition(string key)
        {
            this.key = key;
        }
        
        public string GetKey()
        {
            return this.key;
        }

        public IWorldKey WorldKey { get; }
        public Comparison Comparison { get; }
        public int Amount { get; }
    }
}