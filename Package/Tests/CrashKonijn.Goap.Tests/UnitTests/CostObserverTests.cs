using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using CrashKonijn.Goap.Scriptables;
using CrashKonijn.Goap.UnitTests.Support;
using FluentAssertions;
using LamosInteractive.Goap.Interfaces;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CrashKonijn.Goap.UnitTests
{
    public class CostObserverTests
    {
        [Test]
        public void GetCost_WithoutPath_ReturnsCost()
        {
            // Arrange
            var observer = new CostObserver();
            
            var inputAction = Substitute.For<IActionBase>();
            inputAction.GetCost(Arg.Any<IWorldData>()).Returns(5);
            
            // Act
            var result = observer.GetCost(inputAction, new List<IAction>());

            // Assert
            result.Should().Be(5f);
        }

        [Test]
        public void GetCost_WithoutLastTarget_ReturnsCost()
        {
            // Arrange
            var observer = new CostObserver();
            
            var inputAction = Substitute.For<IActionBase>();
            inputAction.GetCost(Arg.Any<IWorldData>()).Returns(5);
            
            var lastAction = Substitute.For<IActionBase>();
            lastAction.Config.Returns(Substitute.For<ActionConfig>());

            // Act
            var result = observer.GetCost(inputAction, new List<IAction>() { lastAction });

            // Assert
            result.Should().Be(5f);
        }

        [Test]
        public void GetCost_WithLastTarget_ReturnsCostAndDistance()
        {
            // Arrange
            var target = new TargetKeyScriptable();
            var observer = new CostObserver();

            var worldData = new GlobalWorldData();
            worldData.SetTarget(target, null);
            observer.SetWorldData(worldData);
            
            var actionConfig = Substitute.For<ActionConfig>();
            actionConfig.target = target;
            
            var inputAction = Substitute.For<IActionBase>();
            inputAction.GetCost(Arg.Any<IWorldData>()).Returns(5);
            inputAction.GetDistanceCost(Arg.Any<ITarget>(), Arg.Any<ITarget>()).Returns(5);
            inputAction.Config.Returns(actionConfig);
            
            var lastAction = Substitute.For<IActionBase>();
            lastAction.Config.Returns(actionConfig);

            // Act
            var result = observer.GetCost(inputAction, new List<IAction>() { lastAction });

            // Assert
            result.Should().Be(10f);
        }
    }
}