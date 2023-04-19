using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Configs.Interfaces;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests.Validators
{
    public class TargetKeySensorsValidatorTests
    {
        [Test]
        public void Validate_MissingSensor_ShouldReturnWarning()
        {
            // Arrange
            var results = new ValidationResults("test");
            
            var action = Substitute.For<IActionConfig>();
            action.Target.Returns(Substitute.For<ITargetKey>());
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>
            {
                action
            });
            config.TargetSensors.Returns(new List<ITargetSensorConfig>());
            
            var validator = new TargetKeySensorsValidator();
            
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
            
            var targetKey = Substitute.For<ITargetKey>();
            
            var action = Substitute.For<IActionConfig>();
            action.Target.Returns(targetKey);
            
            var sensor = Substitute.For<ITargetSensorConfig>();
            sensor.Key.Returns(targetKey);
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>
            {
                action
            });
            config.TargetSensors.Returns(new List<ITargetSensorConfig>{ sensor });
            
            var validator = new TargetKeySensorsValidator();
            
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
            
            var action = Substitute.For<IActionConfig>();
            action.Target.ReturnsNull();
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>
            {
                action
            });
            config.TargetSensors.Returns(new List<ITargetSensorConfig>());
            
            var validator = new TargetKeySensorsValidator();
            
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
            
            var sensor = Substitute.For<ITargetSensorConfig>();
            sensor.Key.ReturnsNull();
            
            var config = Substitute.For<IGoapSetConfig>();
            config.Actions.Returns(new List<IActionConfig>());
            config.TargetSensors.Returns(new List<ITargetSensorConfig>{ sensor });
            
            var validator = new TargetKeySensorsValidator();
            
            // Act
            validator.Validate(config, results);
            
            // Assert
            results.HasWarnings().Should().BeFalse();
            results.HasErrors().Should().BeFalse();
        }
    }
}