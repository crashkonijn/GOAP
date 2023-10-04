using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Resolver.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using ICondition = CrashKonijn.Goap.Interfaces.ICondition;

namespace CrashKonijn.Goap.UnitTests
{
    public class GoapSetJobRunnerTests
    {
        [Test]
        public void Run_UpdatesSensorRunner()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction>());
            goapSet.GoapConfig.Returns(GoapConfig.Default);

            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).Update();
        }
        
        [Test]
        public void Run_SensesGlobalSensors()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction>());
            goapSet.GoapConfig.Returns(GoapConfig.Default);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
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
            sensorRunner.SenseGlobal().Returns(new GlobalWorldData());
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { });
            goapSet.GoapConfig.Returns(GoapConfig.Default);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(0).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
            resolver.Received(0).StartResolve(Arg.Any<RunData>());
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_Runs()
        {
            // Arrange
            var goal = Substitute.For<IGoalBase>();
            goal.Conditions.Returns(new CrashKonijn.Goap.Resolver.Interfaces.ICondition[] { Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { goal });
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            goapSet.GetActions().Returns(new List<IActionBase>());
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
        }
        
        [Test]
        public void Run_AgentHasCurrentGoalAndDoesHaveAction_Runs()
        {
            // Arrange
            var goal = Substitute.For<IGoalBase>();
            goal.Conditions.Returns(new CrashKonijn.Goap.Resolver.Interfaces.ICondition[] { Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.Returns(Substitute.For<IActionBase>());
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { goal });
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            goapSet.GetActions().Returns(new List<IActionBase>());
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
        }

        [Test]
        public void Run_AgentHasCompletedGoal_CallsGoalCompleteEvent()
        {
            // Arrange
            var goal = Substitute.For<IGoalBase>();
            goal.Conditions.Returns(new CrashKonijn.Goap.Resolver.Interfaces.ICondition[] { Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { goal });
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            goapSet.GetActions().Returns(new List<IActionBase>());

            goapSet.GoapConfig.ConditionObserver.IsMet(Arg.Any<ICondition>()).Returns(true);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
            resolver.Received(0).StartResolve(Arg.Any<RunData>());
            agent.Events.Received(1).GoalCompleted(agent.CurrentGoal);
        }

        [Test]
        public void Run_AgentHasNotCompletedGoal_DoesntCallGoalCompleteEvent()
        {
            // Arrange
            var goal = Substitute.For<IGoalBase>();
            goal.Conditions.Returns(new CrashKonijn.Goap.Resolver.Interfaces.ICondition[] { Substitute.For<ICondition>() });
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { goal });
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            goapSet.GetActions().Returns(new List<IActionBase>());

            goapSet.GoapConfig.ConditionObserver.IsMet(Arg.Any<ICondition>()).Returns(false);
            
            var resolver = Substitute.For<IGraphResolver>();
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
            agent.Events.Received(0).GoalCompleted(agent.CurrentGoal);
        }
        
        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_SetsTheActionOnAgent()
        {
            // Arrange
            var goal = Substitute.For<IGoalBase>();
            goal.Conditions.Returns(new CrashKonijn.Goap.Resolver.Interfaces.ICondition[] { Substitute.For<ICondition>() });

            var action = Substitute.For<IActionBase>();
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { goal });
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            goapSet.GetActions().Returns(new List<IActionBase>());

            var handle = Substitute.For<IResolveHandle>();
            handle.Complete().Returns(new IAction[] { action });
            
            var resolver = Substitute.For<IGraphResolver>();
            resolver.StartResolve(Arg.Any<RunData>()).Returns(handle);
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Complete();
            
            // Assert
            agent.Received(1).SetAction(action, Arg.Any<List<IActionBase>>(), Arg.Any<ITarget>());
        }
        
        [Test]
        public void Run_AgentHasCurrentGoalAndAction_ResolvingSameActionDoesntCallSet()
        {
            // Arrange
            var goal = Substitute.For<IGoalBase>();
            goal.Conditions.Returns(new CrashKonijn.Goap.Resolver.Interfaces.ICondition[] { Substitute.For<ICondition>() });

            var action = Substitute.For<IActionBase>();
            
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.Returns(action);
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { goal });
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            goapSet.GetActions().Returns(new List<IActionBase>());

            var handle = Substitute.For<IResolveHandle>();
            handle.Complete().Returns(new IAction[] { action });
            
            var resolver = Substitute.For<IGraphResolver>();
            resolver.StartResolve(Arg.Any<RunData>()).Returns(handle);
            
            var runner = new GoapSetJobRunner(goapSet, resolver);
            
            // Act
            runner.Run();
            runner.Complete();
            
            // Assert
            agent.Received(0).SetAction(Arg.Any<IActionBase>(), Arg.Any<List<IActionBase>>(), Arg.Any<ITarget>());
        }
    }
}