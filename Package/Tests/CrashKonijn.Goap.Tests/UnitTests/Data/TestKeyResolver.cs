using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class TestKeyResolver : IKeyResolver
    {
        public string GetKey(ICondition condition)
        {
            if (condition is StringCondition stringCondition)
                return stringCondition.GetKey();

            if (condition is TestConnection testConnection)
                return testConnection.GetKey();

            throw new Exception("Should not happen");
        }

        public string GetKey(IEffect effect)
        {
            if (effect is StringEffect stringEffect)
                return stringEffect.GetKey();

            if (effect is TestConnection testConnection)
                return testConnection.GetKey();

            throw new Exception("Should not happen");
        }

        public bool AreConflicting(IEffect effect, ICondition condition)
        {
            return false;
        }

        public void SetWorldData(IWorldData globalWorldData) { }
    }
}
