
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;

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