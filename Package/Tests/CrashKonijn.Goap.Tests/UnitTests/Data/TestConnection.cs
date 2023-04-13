
using CrashKonijn.Goap.Resolver.Interfaces;

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
    }
}