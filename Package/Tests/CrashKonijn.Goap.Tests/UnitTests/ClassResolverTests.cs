using System.Linq;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using CrashKonijn.Goap.UnitTests.Data;
using CrashKonijn.Goap.UnitTests.Support;
using FluentAssertions;
using NUnit.Framework;
using Unity.Collections;

namespace CrashKonijn.Goap.UnitTests
{
    public class ClassResolverTests
    {
        private TestMockFactory factory = new();

        [SetUp]
        public void Init()
        {
            this.factory.Setup<ClassResolver>();
            
            // Unity sometimes thinks that a temporary job is leaking memory
            // This is not the case, so we ignore the message
            // This can trigger in any test, even the ones that don't use the Job system
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }

        [Test]
        public void Load_LoadsGoal()
        {
            // Arrange
            var resolver = this.factory.Get<ClassResolver>();

            // Act
            var result = resolver.Load<IGoal, IGoalConfig>(new[] { GoalConfig.Create<Goal>() });

            // Assert
            result.First().Should().BeOfType<Goal>();
        }

        [Test]
        public void Load_LoadsTargetSensorConfig()
        {
            // Arrange
            var resolver = this.factory.Get<ClassResolver>();

            // Act
            var result = resolver.Load<ITargetSensor, ITargetSensorConfig>(new[] { new TargetSensorConfig<LocalTargetSensor>() });

            // Assert
            result.First().Should().BeOfType<LocalTargetSensor>();
        }

        [Test]
        public void Load_LoadsWorldSensorConfig()
        {
            // Arrange
            var resolver = this.factory.Get<ClassResolver>();

            // Act
            var result = resolver.Load<IWorldSensor, IWorldSensorConfig>(new[] { new WorldSensorConfig<LocalWorldSensor>() });

            // Assert
            result.First().Should().BeOfType<LocalWorldSensor>();
        }
    }
}
