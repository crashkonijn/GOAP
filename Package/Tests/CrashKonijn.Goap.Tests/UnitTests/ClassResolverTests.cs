using System.Linq;
using CrashKonijn.Goap.Configs;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolvers;
using CrashKonijn.Goap.UnitTests.Data;
using CrashKonijn.Goap.UnitTests.Support;
using FluentAssertions;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    public class ClassResolverTests
    {
        private TestMockFactory factory = new();
        
        [SetUp]
        public void Init()
        {
            this.factory.Setup<ClassResolver>();
        }
        
        [Test]
        public void Load_LoadsGoal()
        {
            // Arrange
            var resolver = this.factory.Get<ClassResolver>();

            // Act
            var result = resolver.Load<IGoalBase, IGoalConfig>(new []{ GoalConfig.Create<Goal>() });

            // Assert
            result.First().Should().BeOfType<Goal>();
        }
        
        [Test]
        public void Load_LoadsTargetSensorConfig()
        {
            // Arrange
            var resolver = this.factory.Get<ClassResolver>();

            // Act
            var result = resolver.Load<ITargetSensor, ITargetSensorConfig>(new []{ new TargetSensorConfig<LocalTargetSensor>() });

            // Assert
            result.First().Should().BeOfType<LocalTargetSensor>();
        }
        
        [Test]
        public void Load_LoadsWorldSensorConfig()
        {
            // Arrange
            var resolver = this.factory.Get<ClassResolver>();

            // Act
            var result = resolver.Load<IWorldSensor, IWorldSensorConfig>(new []{ new WorldSensorConfig<LocalWorldSensor>() });

            // Assert
            result.First().Should().BeOfType<LocalWorldSensor>();
        }
    }
}