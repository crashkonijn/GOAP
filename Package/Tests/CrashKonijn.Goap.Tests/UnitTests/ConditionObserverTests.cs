using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Observers;
using CrashKonijn.Goap.Resolver;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace CrashKonijn.Goap.UnitTests
{
    public class ConditionObserverTests
    {
        [SetUp]
        public void Setup()
        {
            // Unity sometimes thinks that a temporary job is leaking memory
            // This is not the case, so we ignore the message
            // This can trigger in any test, even the ones that don't use the Job system
            LogAssert.ignoreFailingMessages = true;
        }
        
        [Test]
        public void IsMet_Positive_IsPresent()
        {
            // Arrange
            var key = new WorldKey("world-key");
            var worldData = new GlobalWorldData();
            worldData.SetState(key, 1);
            
            var observer = new ConditionObserver();
            observer.SetWorldData(worldData);
        
            var condition = new Condition
            {
                Comparison = Comparison.GreaterThanOrEqual,
                Amount = 1,
                WorldKey = key
            };

            // Act
            var result = observer.IsMet(condition);

            // Assert
            Assert.IsTrue(result);
        }
    
        [Test]
        public void IsMet_Positive_IsNotPresent()
        {
            // Arrange
            var key = new WorldKey("world-key");
            var worldData = new GlobalWorldData();
            var observer = new ConditionObserver();
            observer.SetWorldData(worldData);
        
            var condition = new Condition
            {
                Comparison = Comparison.GreaterThanOrEqual,
                Amount = 1,
                WorldKey = key
            };

            // Act
            var result = observer.IsMet(condition);

            // Assert
            Assert.IsFalse(result);
        }
    
        [Test]
        public void IsMet_Negative_IsPresent()
        {
            // Arrange
            var key = new WorldKey("world-key");
            var worldData = new GlobalWorldData();
            worldData.SetState(key, 1);
            
            var observer = new ConditionObserver();
            observer.SetWorldData(worldData);
        
            var condition = new Condition
            {
                Comparison = Comparison.SmallerThan,
                Amount = 1,
                WorldKey = key
            };

            // Act
            var result = observer.IsMet(condition);

            // Assert
            Assert.IsFalse(result);
        }
    
        [Test]
        public void IsMet_Negative_IsNotPresent()
        {
            // Arrange
            var key = new WorldKey("world-key");
            var worldData = new GlobalWorldData();
            var observer = new ConditionObserver();
            observer.SetWorldData(worldData);
        
            var condition = new Condition
            {
                Comparison = Comparison.SmallerThanOrEqual,
                Amount = 0,
                WorldKey = key
            };

            // Act
            var result = observer.IsMet(condition);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
