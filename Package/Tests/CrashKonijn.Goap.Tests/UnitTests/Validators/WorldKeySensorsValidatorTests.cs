using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests.Validators
{
    public class WorldKeySensorsValidatorTests
    {
        [Test]
        public void Validate_MissingSensor_ShouldReturnWarning()
        {
            // Arrange
            var results = new ValidationResults("test");

            var condition = Substitute.For<ICondition>();
            condition.WorldKey.Returns(Substitute.For<IWorldKey>());
            
            var action = Substitute.For<IActionConfig>();
            action.Conditions.Returns(new[] { condition });
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>
            {
                action
            });
            config.WorldSensors.Returns(new List<IWorldSensorConfig>());
            
            var validator = new WorldKeySensorsValidator();
            
            // Act
            validator.Validate(config, results);
            
            // Assert
            results.HasWarnings().Should().BeTrue();
            results.HasErrors().Should().BeFalse();
        }
        
        [Test]
        public void Validate_NoMissingSensor_ShouldReturnNoWarning()
        {
            // Arrange
            var results = new ValidationResults("test");

            var worldKey = Substitute.For<IWorldKey>();
            
            var condition = Substitute.For<ICondition>();
            condition.WorldKey.Returns(worldKey);
            
            var sensor = Substitute.For<IWorldSensorConfig>();
            sensor.Key.Returns(worldKey);
            
            var action = Substitute.For<IActionConfig>();
            action.Conditions.Returns(new[] { condition });
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>
            {
                action
            });
            config.WorldSensors.Returns(new List<IWorldSensorConfig>{ sensor });
            
            var validator = new WorldKeySensorsValidator();
            
            // Act
            validator.Validate(config, results);
            
            // Assert
            results.HasWarnings().Should().BeFalse();
            results.HasErrors().Should().BeFalse();
        }
        
        [Test]
        // This test breaks if the validator doesn't handle null values properly.
        public void Validate_WithNullWorldKey_ShouldReturnNoWarnings()
        {
            // Arrange
            var results = new ValidationResults("test");

            var condition = Substitute.For<ICondition>();
            condition.WorldKey.ReturnsNull();
            
            var action = Substitute.For<IActionConfig>();
            action.Conditions.Returns(new[] { condition });
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>
            {
                action
            });
            config.WorldSensors.Returns(new List<IWorldSensorConfig>());
            
            var validator = new WorldKeySensorsValidator();
            
            // Act
            validator.Validate(config, results);
            
            // Assert
            results.HasWarnings().Should().BeFalse();
            results.HasErrors().Should().BeFalse();
        }
        
        [Test]
        public void Validate_WithNullSensorWorldKey_ShouldReturnNoWarnings()
        {
            // Arrange
            var results = new ValidationResults("test");

            var sensor = Substitute.For<IWorldSensorConfig>();
            sensor.Key.ReturnsNull();
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>());
            config.WorldSensors.Returns(new List<IWorldSensorConfig>{ sensor });
            
            var validator = new WorldKeySensorsValidator();
            
            // Act
            validator.Validate(config, results);
            
            // Assert
            results.HasWarnings().Should().BeFalse();
            results.HasErrors().Should().BeFalse();
        }
    }
}