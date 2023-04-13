using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Observers;
using CrashKonijn.Goap.Resolver;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    public class ConditionObserverTests
    {
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
