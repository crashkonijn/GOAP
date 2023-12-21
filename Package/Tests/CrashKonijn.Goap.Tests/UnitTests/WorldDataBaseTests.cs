using NUnit.Framework;
using NSubstitute;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Configs;

namespace CrashKonijn.Goap.UnitTests
{
    [TestFixture]
    public class WorldDataBaseTests
    {
        private LocalWorldData worldDataBase;

        [SetUp]
        public void SetUp()
        {
            this.worldDataBase = new LocalWorldData();
        }

        [Test]
        public void GetTarget_ReturnsCorrectTarget()
        {
            // Arrange
            var mockAction = Substitute.For<IAction>();
            var mockTarget = Substitute.For<ITarget>();
            mockAction.Config.Target.Returns(new TargetKey("example"));

            this.worldDataBase.SetTarget<TargetKey>(mockTarget);

            // Act
            var target = this.worldDataBase.GetTarget(mockAction);

            // Assert
            Assert.AreEqual(mockTarget, target);
        }

        [Test]
        public void IsTrue_Generic_ReturnsCorrectResult()
        {
            // Arrange
            this.worldDataBase.SetState<WorldKey>(1);
            
            // Act
            var result = this.worldDataBase.IsTrue<WorldKey>(Comparison.GreaterThan, 0);
            
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsTrue_Instance_ReturnsCorrectResult()
        {
            // Arrange
            this.worldDataBase.SetState<WorldKey>(1);
            
            // Act
            var result = this.worldDataBase.IsTrue(new WorldKey("example"), Comparison.GreaterThan, 0);
            
            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void SetState_Generic_AddsStateCorrectly()
        {
            // Act
            this.worldDataBase.SetState<WorldKey>(1);

            // Assert
            Assert.AreEqual(1, this.worldDataBase.States.Count);
            Assert.IsTrue(this.worldDataBase.States.ContainsKey(typeof(WorldKey)));
            Assert.AreEqual(1, this.worldDataBase.States[typeof(WorldKey)]);
        }

        [Test]
        public void SetState_Instance_AddsStateCorrectly()
        {
            // Act
            this.worldDataBase.SetState(new WorldKey("example"),1);

            // Assert
            Assert.AreEqual(1, this.worldDataBase.States.Count);
            Assert.IsTrue(this.worldDataBase.States.ContainsKey(typeof(WorldKey)));
            Assert.AreEqual(1, this.worldDataBase.States[typeof(WorldKey)]);
        }

        [Test]
        public void SetTarget_Generic_AddsTargetCorrectly()
        {
            // Arrange
            var mockTarget = Substitute.For<ITarget>();
            
            // Act
            this.worldDataBase.SetTarget<TargetKey>(mockTarget);

            // Assert
            Assert.AreEqual(1, this.worldDataBase.Targets.Count);
            Assert.IsTrue(this.worldDataBase.Targets.ContainsKey(typeof(TargetKey)));
            Assert.AreEqual(mockTarget, this.worldDataBase.Targets[typeof(TargetKey)]);
        }

        [Test]
        public void SetTarget_Instance_AddsTargetCorrectly()
        {
            // Arrange
            var mockTarget = Substitute.For<ITarget>();
            
            // Act
            this.worldDataBase.SetTarget(new TargetKey("example"), mockTarget);

            // Assert
            Assert.AreEqual(1, this.worldDataBase.Targets.Count);
            Assert.IsTrue(this.worldDataBase.Targets.ContainsKey(typeof(TargetKey)));
            Assert.AreEqual(mockTarget, this.worldDataBase.Targets[typeof(TargetKey)]);
        }

        [Test]
        public void SetParent_UpdatesStatesAndTargetsCorrectly()
        {
            // Arrange
            var mockWorldData = Substitute.For<IGlobalWorldData>();
            mockWorldData.GetWorldValue(Arg.Any<Type>()).Returns((true, 1));
            var mockTarget = Substitute.For<ITarget>();
            mockWorldData.GetTargetValue(Arg.Any<Type>()).Returns(mockTarget);

            // Act
            this.worldDataBase.SetParent(mockWorldData);

            // Assert
            Assert.AreEqual(0, this.worldDataBase.States.Count);
            Assert.IsFalse(this.worldDataBase.States.ContainsKey(typeof(IWorldKey)));
            Assert.AreEqual((true, 1), this.worldDataBase.GetWorldValue(typeof(IWorldKey)));

            Assert.AreEqual(0, this.worldDataBase.Targets.Count);
            Assert.IsFalse(this.worldDataBase.Targets.ContainsKey(typeof(ITargetKey)));
            Assert.AreEqual(mockTarget, this.worldDataBase.GetTargetValue(typeof(ITargetKey)));
        }
    }
}