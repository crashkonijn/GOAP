using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    public class SensorRunnerTests
    {
        // Global
        [Test]
        public void SenseGlobal_WithPositiveWorldSense_IsPresentInStates()
        {
            // Arrange
            var key = new WorldKey("test");
            
            var sensor = Substitute.For<IGlobalWorldSensor>();
            sensor.Sense().Returns((SenseValue) 1);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] { sensor };
            var targetSensors = new ITargetSensor[] { };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            // Act
            var data = runner.SenseGlobal();

            // Assert
            data.States.Should().ContainKey(key);
            data.States[key].Should().Be(1);
        }
        
        [Test]
        public void SenseGlobal_WithNegativeWorldSense_IsPresentInStates()
        {
            // Arrange
            var key = new WorldKey("test");
            
            var sensor = Substitute.For<IGlobalWorldSensor>();
            sensor.Sense().Returns((SenseValue) 0);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] { sensor };
            var targetSensors = new ITargetSensor[] { };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            // Act
            var data = runner.SenseGlobal();

            // Assert
            data.States.Should().ContainKey(key);
            data.States[key].Should().Be(0);
        }
        
        [Test]
        public void SenseGlobal_WithPositiveTargetSense_IsPresentInTargets()
        {
            // Arrange
            var target = Substitute.For<ITarget>();
            var key = new TargetKey("test");
            
            var sensor = Substitute.For<IGlobalTargetSensor>();
            sensor.Sense().Returns(target);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] {  };
            var targetSensors = new ITargetSensor[] { sensor };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            // Act
            var data = runner.SenseGlobal();

            // Assert
            data.Targets.Should().ContainKey(key);
            data.Targets.Should().ContainValue(target);
        }
        
        [Test]
        public void SenseGlobal_WithNegativeTargetSense_IsPresentInTargets()
        {
            // Arrange
            var target = Substitute.For<ITarget>();
            var key = new TargetKey("test");
            
            var sensor = Substitute.For<IGlobalTargetSensor>();
            sensor.Sense().ReturnsNull();
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] {  };
            var targetSensors = new ITargetSensor[] { sensor };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            // Act
            var data = runner.SenseGlobal();

            // Assert
            data.Targets.Should().ContainKey(key);
            data.Targets.Should().NotContainValue(target);
        }
        
        // Local
        [Test]
        public void SenseLocal_WithPositiveWorldSense_IsPresentInStates()
        {
            // Arrange
            var key = new WorldKey("test");
            
            var sensor = Substitute.For<ILocalWorldSensor>();
            sensor.Sense(Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>()).Returns((SenseValue) 1);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] { sensor };
            var targetSensors = new ITargetSensor[] { };
            
            var runner = new SensorRunner(worldSensors, targetSensors);
            
            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());

            // Act
            var data = runner.SenseLocal(new GlobalWorldData(), agent);

            // Assert
            data.States.Should().ContainKey(key);
            data.States[key].Should().Be(1);
        }
        
        [Test]
        public void SenseLocal_WithNegativeWorldSense_IsNotPresentInStates()
        {
            // Arrange
            var key = new WorldKey("test");
            
            var sensor = Substitute.For<ILocalWorldSensor>();
            sensor.Sense(Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>()).Returns((SenseValue) 0);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] { sensor };
            var targetSensors = new ITargetSensor[] { };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());

            // Act
            var data = runner.SenseLocal(new GlobalWorldData(), agent);

            // Assert
            data.States.Should().ContainKey(key);
            data.States[key].Should().Be(0);
        }
        
        [Test]
        public void SenseLocal_WithPositiveTargetSense_IsPresentInTargets()
        {
            // Arrange
            var target = Substitute.For<ITarget>();
            var key = new TargetKey("test");
            
            var sensor = Substitute.For<ILocalTargetSensor>();
            sensor.Sense(Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>()).Returns(target);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] {  };
            var targetSensors = new ITargetSensor[] { sensor };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());
            
            // Act
            var data = runner.SenseLocal(new GlobalWorldData(), agent);

            // Assert
            data.Targets.Should().ContainKey(key);
            data.Targets.Should().ContainValue(target);
        }
        
        [Test]
        public void SenseLocal_WithNegativeTargetSense_IsPresentInTargets()
        {
            // Arrange
            var target = Substitute.For<ITarget>();
            var key = new TargetKey("test");
            
            var sensor = Substitute.For<ILocalTargetSensor>();
            sensor.Sense(Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>()).ReturnsNull();
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] {  };
            var targetSensors = new ITargetSensor[] { sensor };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());
            
            // Act
            var data = runner.SenseLocal(new GlobalWorldData(), agent);

            // Assert
            data.Targets.Should().ContainKey(key);
            data.Targets.Should().NotContainValue(target);
        }
        
        [Test]
        public void SenseLocal_ContainsGlobalTarget()
        {
            // Arrange
            var target = Substitute.For<ITarget>();
            var key = new TargetKey("test");
            
            var sensor = Substitute.For<IGlobalTargetSensor>();
            sensor.Sense().Returns(target);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] {  };
            var targetSensors = new ITargetSensor[] { sensor };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());

            var globalWorldData = new GlobalWorldData();
            globalWorldData.Targets.Add(key, target);
            
            // Act
            var data = runner.SenseLocal(globalWorldData, agent);

            // Assert
            data.Targets.Should().ContainKey(key);
            data.Targets.Should().ContainValue(target);
        }
        
        [Test]
        public void SenseLocal_ContainsGlobalState()
        {
            // Arrange
            var key = new WorldKey("test");
            
            var sensor = Substitute.For<ILocalWorldSensor>();
            sensor.Sense(Arg.Any<IMonoAgent>(), Arg.Any<IComponentReference>()).Returns((SenseValue) 0);
            sensor.Key.Returns(key);
            
            var worldSensors = new IWorldSensor[] { };
            var targetSensors = new ITargetSensor[] { };
            
            var runner = new SensorRunner(worldSensors, targetSensors);

            var agent = Substitute.For<IMonoAgent>();
            agent.WorldData.Returns(new LocalWorldData());
            
            var globalWorldData = new GlobalWorldData();
            globalWorldData.States.Add(key, 1);

            // Act
            var data = runner.SenseLocal(globalWorldData, agent);

            // Assert
            data.States.Should().ContainKey(key);
            data.States[key].Should().Be(1);
        }
    }
}