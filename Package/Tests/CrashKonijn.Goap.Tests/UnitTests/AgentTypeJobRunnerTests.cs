using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using ICondition = CrashKonijn.Goap.Core.Interfaces.ICondition;

namespace CrashKonijn.Goap.UnitTests
{
    public class AgentTypeJobRunnerTests
    {
        [Test]
        public void Run_UpdatesSensorRunner()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable>());
            agentType.GoapConfig.Returns(GoapConfig.Default);

            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { Substitute.For<IMonoAgent>() });
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).Update();
        }
        
        [Test]
        public void Run_WithAtLeastOneAgent_DoesntSenseGlobalSensors()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable>());
            agentType.GoapConfig.Returns(GoapConfig.Default);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>());
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(0).SenseGlobal();
        }
        
        [Test]
        public void Run_WithNoAgents_SensesGlobalSensors()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable>());
            agentType.GoapConfig.Returns(GoapConfig.Default);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { Substitute.For<IMonoAgent>() });
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseGlobal();
        }

        [Test]
        public void Run_AgentHasNoCurrentGoal_DoesNotRun()
        {
            // Arrange
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.ReturnsNull();
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable> { });
            agentType.GoapConfig.Returns(GoapConfig.Default);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>());
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(0).SenseLocal(agent);
            resolver.Received(0).StartResolve(Arg.Any<RunData>());
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_Runs()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new [] { Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            agentType.GetActions().Returns(new List<IAction>());
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { agent });
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(agent);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
        }
        
        [Test]
        public void Run_AgentHasCurrentGoalAndDoesHaveAction_Runs()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new []{ Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.Returns(Substitute.For<IAction>());
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            agentType.GetActions().Returns(new List<IAction>());
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { agent });
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(agent);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
        }

        [Test]
        public void Run_AgentHasCompletedGoal_CallsGoalCompleteEvent()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new [] { Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            agentType.GetActions().Returns(new List<IAction>());

            agentType.GoapConfig.ConditionObserver.IsMet(Arg.Any<ICondition>()).Returns(true);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { agent });
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(agent);
            resolver.Received(0).StartResolve(Arg.Any<RunData>());
            agent.Events.Received(1).GoalCompleted(agent.CurrentGoal);
        }

        [Test]
        public void Run_AgentHasNotCompletedGoal_DoesntCallGoalCompleteEvent()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new [] { Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            agentType.GetActions().Returns(new List<IAction>());

            agentType.GoapConfig.ConditionObserver.IsMet(Arg.Any<ICondition>()).Returns(false);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { agent });
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(agent);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
            agent.Events.Received(0).GoalCompleted(agent.CurrentGoal);
        }
        
        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_SetsTheActionOnAgent()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new [] { Substitute.For<ICondition>() });

            var action = Substitute.For<IAction>();
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            agentType.GetActions().Returns(new List<IAction>());

            var handle = Substitute.For<IResolveHandle>();
            handle.Complete().Returns(new IConnectable[] { action });
            
            var resolver = Substitute.For<IGraphResolver>();
            resolver.StartResolve(Arg.Any<RunData>()).Returns(handle);
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { agent });
            runner.Complete();
            
            // Assert
            agent.Received(1).SetAction(action, Arg.Any<List<IAction>>(), Arg.Any<ITarget>());
        }
        
        [Test]
        public void Run_AgentHasCurrentGoalAndAction_ResolvingSameActionDoesntCallSet()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new [] { Substitute.For<ICondition>() });

            var action = Substitute.For<IAction>();
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.Returns(action);
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var agentType = Substitute.For<IAgentType>();
            agentType.SensorRunner.Returns(sensorRunner);
            agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            agentType.GetActions().Returns(new List<IAction>());

            var handle = Substitute.For<IResolveHandle>();
            handle.Complete().Returns(new IConnectable[] { action });
            
            var resolver = Substitute.For<IGraphResolver>();
            resolver.StartResolve(Arg.Any<RunData>()).Returns(handle);
            
            var runner = new AgentTypeJobRunner(agentType, resolver);
            
            // Act
            runner.Run(new HashSet<IMonoAgent>() { agent });
            runner.Complete();
            
            // Assert
            agent.Received(0).SetAction(Arg.Any<IAction>(), Arg.Any<List<IAction>>(), Arg.Any<ITarget>());
        }
    }
}