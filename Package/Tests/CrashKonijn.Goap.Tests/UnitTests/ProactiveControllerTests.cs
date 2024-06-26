using System.Collections.Generic;
using CrashKonijn.Goap.Classes.Controllers;
using CrashKonijn.Goap.Core.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    [TestFixture]
    public class ProactiveControllerTests
    {
        private IGoap goap;
        private ProactiveController proactiveController;
        private IGoapEvents events;
        private Dictionary<IAgentType, IAgentTypeJobRunner> typeRunners;
        private IMonoGoapAgent agent;
        private IAgentType agentType;
        private IAgentTypeJobRunner runner;

        [SetUp]
        public void SetUp()
        {
            this.agentType = Substitute.For<IAgentType>();
            this.agentType.SensorRunner.Returns(Substitute.For<ISensorRunner>());
            
            this.agent = Substitute.For<IMonoGoapAgent>();
            this.agent.AgentType.Returns(this.agentType);
            
            this.events = Substitute.For<IGoapEvents>();
            this.runner = Substitute.For<IAgentTypeJobRunner>();
            
            this.typeRunners = new Dictionary<IAgentType, IAgentTypeJobRunner>()
            {
                { this.agentType, this.runner }
            };
            
            this.goap = Substitute.For<IGoap>();
            this.goap.Agents.Returns(new List<IMonoGoapAgent>());
            this.goap.AgentTypeRunners.Returns(this.typeRunners);
            this.goap.Events.Returns(this.events);
        
            this.proactiveController = new ProactiveController();
        }

        [Test]
        public void OnUpdate_ResolveActionCalledWhenResolveTimeExpired()
        {
            // Arrange
            this.agent.Agent.Timers.Resolve.IsRunningFor(this.proactiveController.ResolveTime).Returns(true);
        
            this.goap.Agents.Returns(new List<IMonoGoapAgent>() { this.agent });
            this.proactiveController.Initialize(this.goap);

            // Act
            this.proactiveController.OnUpdate();

            // Assert
            this.agent.Received().ResolveAction();
        }
        
        [Test]
        public void OnUpdate_ResolveActionNotCalledWhenResolveTimeNotExpired()
        {
            // Arrange
            this.agent = Substitute.For<IMonoGoapAgent>();
            this.agent.Agent.Timers.Resolve.IsRunningFor(this.proactiveController.ResolveTime).Returns(false);
        
            this.goap.Agents.Returns(new List<IMonoGoapAgent>() { this.agent });
            this.proactiveController.Initialize(this.goap);

            // Act
            this.proactiveController.OnUpdate();

            // Assert
            this.agent.DidNotReceive().ResolveAction();
        }

        [Test]
        public void OnUpdate_RunCalledOnAgentTypeRunners()
        {
            // Arrange
            this.proactiveController.Initialize(this.goap);
        
            // Act
            this.proactiveController.OnUpdate();
        
            // Assert
            this.runner.Received().Run(Arg.Any<HashSet<IMonoGoapAgent>>());
        }
        
        [Test]
        public void OnLateUpdate_CompleteCalledOnAgentTypeRunners()
        {
            // Arrange
            this.proactiveController.Initialize(this.goap);
        
            // Act
            this.proactiveController.OnLateUpdate();
        
            // Assert
            this.runner.Received().Complete();
        }
    }
}
