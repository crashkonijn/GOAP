using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Controllers;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Core.Interfaces;
using NSubstitute;
using NUnit.Framework;

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
        }
        
        [Test]
        public void OnAgentResolve_CallsRunAndComplete()
        {
            // Arrange
            var agentType = Substitute.For<IAgentType>();
            
            var agent = Substitute.For<IAgent>();
            agent.AgentType.Returns(agentType);

            var jobRunner = Substitute.For<IAgentTypeJobRunner>();
            var typeRunners = new Dictionary<IAgentType, IAgentTypeJobRunner>()
            {
                { agentType, jobRunner }
            };
            
            this.goap.AgentTypeRunners.Returns(typeRunners);
            
            // Act
            this.goap.Events.OnAgentResolve += Raise.Event<AgentDelegate>(agent);
            
            // Assert
            jobRunner.Received(1).Run(Arg.Any<HashSet<IMonoAgent>>());
            jobRunner.Received(1).Complete();
        }
    }
}