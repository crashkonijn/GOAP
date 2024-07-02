using System;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class TestKeyResolver : IKeyResolver
    {
        public string GetKey(IConnectable action, ICondition condition)
        {
            if (condition is StringCondition stringCondition)
                return stringCondition.GetKey();
            
            if (condition is TestConnection testConnection)
                return testConnection.GetKey();

            throw new Exception("Should not happen");
        }

        public string GetKey(IConnectable action, IEffect effect)
        {
            if (effect is StringEffect stringEffect)
                return stringEffect.GetKey();
            
            if (effect is TestConnection testConnection)
                return testConnection.GetKey();

            throw new Exception("Should not happen");
        }

        public void SetWorldData(IWorldData globalWorldData)
        {
        }
    }
}