using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using CrashKonijn.Goap.UnitTests.Support;
using NSubstitute;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

namespace CrashKonijn.Goap.UnitTests
{
    public class GoapActionProviderTests
    {
        [SetUp]
        public void Init()
        {
            // Unity sometimes thinks that a temporary job is leaking memory
            // This is not the case, so we ignore the message
            // This can trigger in any test, even the ones that don't use the Job system
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }

        [Test]
        public void SetAction_ShouldThrowGoalStartEvent()
        {
            // Arrange
            var gameObject = new GameObject();
            var provider = gameObject.AddComponent<GoapActionProvider>();

            var logger = provider.MockLogger();
            logger.ShouldLog().Returns(false);

            var events = provider.MockEvents();
            provider.Receiver = Substitute.For<IActionReceiver>();

            provider.MockWorldData();

            var goal = Substitute.For<IGoal>();

            var result = Substitute.For<IGoalResult>();
            result.Goal.Returns(goal);

            // Act
            provider.SetAction(result);

            // Assert
            events.Received().GoalStart(goal);
        }
    }
}
