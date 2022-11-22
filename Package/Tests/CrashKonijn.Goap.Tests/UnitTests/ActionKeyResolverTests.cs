using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Resolvers;
using CrashKonijn.Goap.Scriptables;
using NUnit.Framework;
using Packages.LamosInteractive.Goap.Unity.Tests.UnitTests.Data;
using UnityEngine;
using UnityEngine.TestTools;

public class ActionKeyResolverTests
{
    // A Test behaves as an ordinary method
    [TestCase(true, "_true")]
    [TestCase(false, "_false")]
    public void GetKey_ActionCondition_IsCorrect(bool positive, string expected)
    {
        // Arrange
        var resolver = new KeyResolver();
        var condition = new Condition
        {
            positive = positive,
            worldKey = new WorldKey()
        };

        // Act
        var result = resolver.GetKey(new Action(), condition);

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    // A Test behaves as an ordinary method
    [TestCase(true, "_true")]
    [TestCase(false, "_false")]
    public void GetKey_ActionEffect_IsCorrect(bool positive, string expected)
    {
        // Arrange
        var resolver = new KeyResolver();
        var condition = new Effect
        {
            positive = positive,
            worldKey = new WorldKey()
        };

        // Act
        var result = resolver.GetKey(new Action(), condition);

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    // A Test behaves as an ordinary method
    [TestCase(true, "_true")]
    [TestCase(false, "_false")]
    public void GetKey_GoalCondition_IsCorrect(bool positive, string expected)
    {
        // Arrange
        var resolver = new KeyResolver();
        var condition = new Condition
        {
            positive = positive,
            worldKey = new WorldKey()
        };

        // Act
        var result = resolver.GetKey(new Goal(), condition);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
