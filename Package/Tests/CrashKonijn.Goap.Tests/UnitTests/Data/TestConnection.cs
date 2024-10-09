using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class TestConnection : ICondition, IEffect
    {
        private readonly string key;

        public TestConnection(string key)
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
        public bool Increase { get; }
    }
}
