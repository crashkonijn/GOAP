using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Observers;
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
            worldData.SetState(key, WorldKeyState.True);
            
            var observer = new ConditionObserver();
            observer.SetWorldData(worldData);
        
            var condition = new Condition
            {
                Positive = true,
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
                Positive = true,
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
            worldData.SetState(key, WorldKeyState.True);
            
            var observer = new ConditionObserver();
            observer.SetWorldData(worldData);
        
            var condition = new Condition
            {
                Positive = false,
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
                Positive = false,
                WorldKey = key
            };

            // Act
            var result = observer.IsMet(condition);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
