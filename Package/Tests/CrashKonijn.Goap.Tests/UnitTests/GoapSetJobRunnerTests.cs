using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.Runners;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Resolver.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

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
            
            var runner = new GoapSetJobRunner(goapSet);
            
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
            
            var runner = new GoapSetJobRunner(goapSet);
            
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
            
            var runner = new GoapSetJobRunner(goapSet);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(0).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_Runs()
        {
            // Arrange
            var agent = Substitute.For<IMonoAgent>();
            var goal = Substitute.For<IGoalBase>();
            agent.CurrentGoal.Returns(goal);
            agent.CurrentAction.ReturnsNull();
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            sensorRunner.SenseGlobal().Returns(new GlobalWorldData());
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { goal });
            goapSet.GoapConfig.Returns(GoapConfig.Default);
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            goapSet.GetActions().Returns(new List<IActionBase>());
            
            var runner = new GoapSetJobRunner(goapSet);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(1).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndAction_DoesNotRun()
        {
            // Arrange
            var agent = Substitute.For<IMonoAgent>();
            agent.CurrentGoal.Returns(Substitute.For<IGoalBase>());
            agent.CurrentAction.Returns(Substitute.For<IActionBase>());
            
            var sensorRunner = Substitute.For<ISensorRunner>();
            sensorRunner.SenseGlobal().Returns(new GlobalWorldData());
            
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.SensorRunner.Returns(sensorRunner);
            goapSet.GetAllNodes().Returns(new List<IAction> { });
            goapSet.GoapConfig.Returns(GoapConfig.Default);
            goapSet.Agents.GetQueue().Returns(new []{ agent });
            
            var runner = new GoapSetJobRunner(goapSet);
            
            // Act
            runner.Run();
            runner.Dispose();
            
            // Assert
            sensorRunner.Received(0).SenseLocal(Arg.Any<GlobalWorldData>(), agent);
        }
    }
}