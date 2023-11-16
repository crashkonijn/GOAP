using NUnit.Framework;
using NSubstitute;
using CrashKonijn.Goap;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Configs;

namespace CrashKonijn.Goap.UnitTests
{
    public class TestWorldDataBase : WorldDataBase
    {
    }
    

    [TestFixture]
    public class WorldDataBaseTests
    {
        private TestWorldDataBase worldDataBase;

        [SetUp]
        public void SetUp()
        {
            this.worldDataBase = new TestWorldDataBase();
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
        public void Apply_UpdatesStatesAndTargetsCorrectly()
        {
            // Arrange
            var mockWorldData = Substitute.For<IWorldData>();
            mockWorldData.States.Returns(new Dictionary<Type, int> { { typeof(IWorldKey), 1 } });
            var mockTarget = Substitute.For<ITarget>();
            mockWorldData.Targets.Returns(new Dictionary<Type, ITarget> { { typeof(ITargetKey), mockTarget } });

            // Act
            this.worldDataBase.Apply(mockWorldData);

            // Assert
            Assert.AreEqual(1, this.worldDataBase.States.Count);
            Assert.IsTrue(this.worldDataBase.States.ContainsKey(typeof(IWorldKey)));
            Assert.AreEqual(1, this.worldDataBase.States[typeof(IWorldKey)]);

            Assert.AreEqual(1, this.worldDataBase.Targets.Count);
            Assert.IsTrue(this.worldDataBase.Targets.ContainsKey(typeof(ITargetKey)));
            Assert.AreEqual(mockTarget, this.worldDataBase.Targets[typeof(ITargetKey)]);
        }
    }
}