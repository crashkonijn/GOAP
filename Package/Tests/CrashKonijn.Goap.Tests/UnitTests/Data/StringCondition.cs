
using CrashKonijn.Goap.Resolver.Interfaces;

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
    }
}