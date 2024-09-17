using System.Collections.Generic;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using NUnit.Framework;

namespace CrashKonijn.Goap.UnitTests
{
    [TestFixture]
    public class ReactiveControllerTests
    {
        private IGoap goap;
        private ReactiveController controller;
        private IGoapEvents events;
        private Dictionary<IAgentType, IAgentTypeJobRunner> typeRunners;
        private IMonoGoapActionProvider actionProvider;
        private IAgentType agentType;
        private IAgentTypeJobRunner runner;

        [SetUp]
        public void SetUp()
        {
            this.agentType = Substitute.For<IAgentType>();
            
            this.actionProvider = Substitute.For<IMonoGoapActionProvider>();
            this.actionProvider.AgentType.Returns(this.agentType);
            
            this.events = Substitute.For<IGoapEvents>();
            this.runner = Substitute.For<IAgentTypeJobRunner>();
            
            this.typeRunners = new Dictionary<IAgentType, IAgentTypeJobRunner>()
            {
                { this.agentType, this.runner }
            };
            
            this.goap = Substitute.For<IGoap>();
            this.goap.Agents.Returns(new List<IMonoGoapActionProvider>());
            this.goap.AgentTypeRunners.Returns(this.typeRunners);
            this.goap.Events.Returns(this.events);
        
            this.controller = new ReactiveController();
        }

        [Test]
        public void OnUpdate_RunCalledOnAgentTypeRunners()
        {
            // Arrange
            this.controller.Initialize(this.goap);
        
            // Act
            this.controller.OnUpdate();
        
            // Assert
            this.runner.Received().Run(Arg.Any<IMonoGoapActionProvider[]>());
        }
        
        [Test]
        public void OnLateUpdate_CompleteCalledOnAgentTypeRunners()
        {
            // Arrange
            this.controller.Initialize(this.goap);
        
            // Act
            this.controller.OnLateUpdate();
        
            // Assert
            this.runner.Received().Complete();
        }
    }
}