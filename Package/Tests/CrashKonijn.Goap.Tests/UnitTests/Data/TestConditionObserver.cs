using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Core;

namespace CrashKonijn.Goap.UnitTests.Data
{
    public class TestConditionObserver : IConditionObserver
    {
        private readonly List<string> metConditionActions;
        
        public TestConditionObserver(List<string> actionKeys)
        {
            this.metConditionActions = actionKeys;
        }
        
        public bool IsMet(ICondition condition)
        {
            if (condition is StringCondition stringCondition)
                return this.metConditionActions.Contains(stringCondition.GetKey());
            
            if (condition is TestConnection testConnection)
                return this.metConditionActions.Contains(testConnection.GetKey());
            
            throw new Exception("Invalid condition");
        }

        public void SetWorldData(IWorldData worldData)
        {
            throw new NotImplementedException();
        }

        public bool IsMet(IEffect effect)
        {
            throw new NotImplementedException();
        }
    }
}