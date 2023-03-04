using System;
using CrashKonijn.Goap.Resolver.Interfaces;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class TestKeyResolver : IActionKeyResolver
    {
        public string GetKey(IAction action, ICondition condition)
        {
            if (condition is StringCondition stringCondition)
                return stringCondition.GetKey();
            
            if (condition is TestConnection testConnection)
                return testConnection.GetKey();

            throw new Exception("Should not happen");
        }

        public string GetKey(IAction action, IEffect effect)
        {
            if (effect is StringEffect stringEffect)
                return stringEffect.GetKey();
            
            if (effect is TestConnection testConnection)
                return testConnection.GetKey();

            throw new Exception("Should not happen");
        }
    }
}