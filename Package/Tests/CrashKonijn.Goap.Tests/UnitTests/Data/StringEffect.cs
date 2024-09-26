using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class StringEffect : IEffect
    {
        private readonly string key;

        public StringEffect(string key)
        {
            this.key = key;
        }

        public string GetKey()
        {
            return this.key;
        }

        public IWorldKey WorldKey { get; }
        public bool Increase { get; }
    }
}
