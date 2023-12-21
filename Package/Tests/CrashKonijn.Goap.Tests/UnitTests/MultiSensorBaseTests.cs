using NUnit.Framework;
using NSubstitute;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Sensors;
using System;

namespace CrashKonijn.Goap.UnitTests
{
    public class TestMultiSensorBase : MultiSensorBase
    {
        public override void Created() { }
        public override void Update() { }
    }

    [TestFixture]
    public class MultiSensorBaseTests
    {
        private TestMultiSensorBase multiSensorBase;
        private IWorldData mockWorldData;
        private IMonoAgent mockMonoAgent;
        private IComponentReference mockComponentReference;

        [SetUp]
        public void SetUp()
        {
            this.multiSensorBase = new TestMultiSensorBase();
            this.mockWorldData = Substitute.For<IWorldData>();
            this.mockMonoAgent = Substitute.For<IMonoAgent>();
            this.mockComponentReference = Substitute.For<IComponentReference>();
        }

        [Test]
        public void AddLocalAndGlobalWorldSensor_AddsSensorsCorrectly()
        {
            // Act
            this.multiSensorBase.AddLocalWorldSensor<IWorldKey>((_, _) => new SenseValue());
            this.multiSensorBase.AddGlobalWorldSensor<IWorldKey>(() => new SenseValue());

            // Assert
            Assert.AreEqual(1, this.multiSensorBase.LocalSensors.Count);
            Assert.AreEqual(1, this.multiSensorBase.GlobalSensors.Count);
        }

        [Test]
        public void AddLocalAndGlobalTargetSensor_AddsSensorsCorrectly()
        {
            // Act
            this.multiSensorBase.AddLocalTargetSensor<ITargetKey>((_, _) => null);
            this.multiSensorBase.AddGlobalTargetSensor<ITargetKey>(() => null);

            // Assert
            Assert.AreEqual(1, this.multiSensorBase.LocalSensors.Count);
            Assert.AreEqual(1, this.multiSensorBase.GlobalSensors.Count);
        }

        [Test]
        public void Sense_CallsLocalAndGlobalSensors()
        {
            // Arrange
            var localSensor = Substitute.For<Func<IMonoAgent, IComponentReference, SenseValue>>();
            var globalSensor = Substitute.For<Func<SenseValue>>();
            
            this.multiSensorBase.AddLocalWorldSensor<IWorldKey>(localSensor);
            this.multiSensorBase.AddGlobalWorldSensor<IWorldKey>(globalSensor);

            // Act
            this.multiSensorBase.Sense(this.mockWorldData, this.mockMonoAgent, this.mockComponentReference);
            this.multiSensorBase.Sense(this.mockWorldData);

            // Assert
            localSensor.Received(1).Invoke(this.mockMonoAgent, this.mockComponentReference);
            globalSensor.Received(1).Invoke();
        }

        [Test]
        public void GetSensors_ReturnsCorrectSensorNames()
        {
            // Arrange
            this.multiSensorBase.AddLocalWorldSensor<IWorldKey>((_, _) => new SenseValue());
            this.multiSensorBase.AddGlobalWorldSensor<IWorldKey>(() => new SenseValue());
            
            // Act
            var sensorNames = this.multiSensorBase.GetSensors();

            // Assert
            Assert.AreEqual(2, sensorNames.Length);
            Assert.AreEqual("IWorldKey (local)", sensorNames[0]);
            Assert.AreEqual("IWorldKey (global)", sensorNames[1]);
        }
        
        [Test]
        public void Sense_AddsLocalDataCorrectly()
        {
            // Arrange
            var worldValue = new SenseValue();
            var targetValue = Substitute.For<ITarget>();

            this.multiSensorBase.AddLocalWorldSensor<IWorldKey>((_, _) => worldValue);
            this.multiSensorBase.AddLocalTargetSensor<ITargetKey>((_, _) => targetValue);

            // Act
            this.multiSensorBase.Sense(this.mockWorldData, this.mockMonoAgent, this.mockComponentReference);

            // Assert
            this.mockWorldData.Received(1).SetState<IWorldKey>(worldValue);
            this.mockWorldData.Received(1).SetTarget<ITargetKey>(targetValue);
        }
        
        [Test]
        public void Sense_AddsGlobalDataCorrectly()
        {
            // Arrange
            var worldValue = new SenseValue();
            var targetValue = Substitute.For<ITarget>();

            this.multiSensorBase.AddGlobalWorldSensor<IWorldKey>(() => worldValue);
            this.multiSensorBase.AddGlobalTargetSensor<ITargetKey>(() => targetValue);

            // Act
            this.multiSensorBase.Sense(this.mockWorldData);

            // Assert
            this.mockWorldData.Received(1).SetState<IWorldKey>(worldValue);
            this.mockWorldData.Received(1).SetTarget<ITargetKey>(targetValue);
        }
    }
}