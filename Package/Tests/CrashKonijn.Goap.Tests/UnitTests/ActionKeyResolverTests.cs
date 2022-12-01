using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolvers;
using NSubstitute;
using NUnit.Framework;

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
            worldKey = Substitute.For<IWorldKey>()
        };

        // Act
        var result = resolver.GetKey(Substitute.For<IActionBase>(), condition);

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
            worldKey = Substitute.For<IWorldKey>()
        };

        // Act
        var result = resolver.GetKey(Substitute.For<IActionBase>(), condition);

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
            worldKey = Substitute.For<IWorldKey>()
        };

        // Act
        var result = resolver.GetKey(Substitute.For<IGoalBase>(), condition);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
