using System.Collections.Generic;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Observers;
using NUnit.Framework;


public class ConditionObserverTests
{
    [Test]
    public void IsMet_Positive_IsPresent()
    {
        // Arrange
        var key = new WorldKey("world-key");
        var worldData = new GlobalWorldData
        {
            States = new HashSet<IWorldKey>{ key }
        };
        var observer = new ConditionObserver();
        observer.SetWorldData(worldData);
        
        var condition = new Condition
        {
            positive = true,
            worldKey = key
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
        var worldData = new GlobalWorldData
        {
            States = new HashSet<IWorldKey>()
        };
        var observer = new ConditionObserver();
        observer.SetWorldData(worldData);
        
        var condition = new Condition
        {
            positive = true,
            worldKey = key
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
        var worldData = new GlobalWorldData
        {
            States = new HashSet<IWorldKey>{ key }
        };
        var observer = new ConditionObserver();
        observer.SetWorldData(worldData);
        
        var condition = new Condition
        {
            positive = false,
            worldKey = key
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
        var worldData = new GlobalWorldData
        {
            States = new HashSet<IWorldKey>()
        };
        var observer = new ConditionObserver();
        observer.SetWorldData(worldData);
        
        var condition = new Condition
        {
            positive = false,
            worldKey = key
        };

        // Act
        var result = observer.IsMet(condition);

        // Assert
        Assert.IsTrue(result);
    }
}
