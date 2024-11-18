using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using NUnit.Framework;
using Unity.Collections;

namespace CrashKonijn.Goap.UnitTests
{
    [TestFixture]
    public class ManualControllerTests
    {
        private IGoap goap;
        private ManualController manualController;
        private IGoapEvents events;

        [SetUp]
        public void SetUp()
        {
            this.events = Substitute.For<IGoapEvents>();

            this.goap = Substitute.For<IGoap>();
            this.goap.Events.Returns(this.events);

            this.manualController = new ManualController();
            this.manualController.Initialize(this.goap);
            
            // Unity sometimes thinks that a temporary job is leaking memory
            // This is not the case, so we ignore the message
            // This can trigger in any test, even the ones that don't use the Job system
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }

        [Test]
        public void OnAgentResolve_CallsRunAndComplete()
        {
            // Arrange
            var agentType = Substitute.For<IAgentType>();

            var agent = Substitute.For<IMonoGoapActionProvider>();
            agent.AgentType.Returns(agentType);

            var jobRunner = Substitute.For<IAgentTypeJobRunner>();
            var typeRunners = new Dictionary<IAgentType, IAgentTypeJobRunner>()
            {
                { agentType, jobRunner },
            };

            this.goap.AgentTypeRunners.Returns(typeRunners);

            // Act
            this.goap.Events.OnAgentResolve += Raise.Event<GoapAgentDelegate>(agent);

            // Assert
            jobRunner.Received(1).Run(Arg.Any<IMonoGoapActionProvider[]>());
            jobRunner.Received(1).Complete();
        }
    }
}
