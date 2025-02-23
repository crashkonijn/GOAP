﻿using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Resolver;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;
using ICondition = CrashKonijn.Goap.Core.ICondition;

namespace CrashKonijn.Goap.UnitTests
{
    public class AgentTypeJobRunnerTests
    {
        private IMonoGoapActionProvider goapActionProvider;
        private IAgent agent;
        private IAgentType agentType;

        [SetUp]
        public void Setup()
        {
            this.agent = Substitute.For<IMonoAgent>();
            this.agent.ActionState.RunState.MayResolve(this.agent).Returns(true);

            this.goapActionProvider = Substitute.For<IMonoGoapActionProvider>();
            this.goapActionProvider.Receiver.Returns(this.agent);
            this.goapActionProvider.Position.Returns(Vector3.zero);

            this.agentType = Substitute.For<IAgentType>();

            this.goapActionProvider.AgentType.Returns(this.agentType);
            
            // Unity sometimes thinks that a temporary job is leaking memory
            // This is not the case, so we ignore the message
            // This can trigger in any test, even the ones that don't use the Job system
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }

        [Test]
        public void Run_UpdatesSensorRunner()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable>());
            this.agentType.GoapConfig.Returns(GoapConfig.Default);
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>());

            var goalRequest = Substitute.For<IGoalRequest>();
            goalRequest.Goals.Returns(new List<IGoal>());

            this.goapActionProvider.GoalRequest.Returns(goalRequest);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).Update();
        }

        [Test]
        public void Run_WithNoAgens_SensesGlobalSensors()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();
            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable>());
            this.agentType.GoapConfig.Returns(GoapConfig.Default);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(Array.Empty<IMonoGoapActionProvider>());
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseGlobal();
        }

        [Test]
        public void Run_WithAtLeastOneAgent_SensesGlobalSensors()
        {
            // Arrange
            var sensorRunner = Substitute.For<ISensorRunner>();
            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable>());
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>());
            this.agentType.GoapConfig.Returns(GoapConfig.Default);

            var goalRequest = Substitute.For<IGoalRequest>();
            goalRequest.Goals.Returns(new List<IGoal>());

            this.goapActionProvider.GoalRequest.Returns(goalRequest);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseGlobal();
        }

        [Test]
        public void Run_AgentHasNoCurrentGoalRequest_DoesNotRun()
        {
            // Arrange
            this.goapActionProvider.CurrentPlan.ReturnsNull();
            this.goapActionProvider.GoalRequest.ReturnsNull();
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { });
            this.agentType.GoapConfig.Returns(GoapConfig.Default);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(Array.Empty<IMonoGoapActionProvider>());
            runner.Dispose();

            // Assert
            sensorRunner.Received(0).SenseLocal(this.goapActionProvider);
            resolver.Received(0).StartResolve(Arg.Any<RunData>());
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_Runs()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var goalResult = Substitute.For<IGoalResult>();
            goalResult.Goal.Returns(goal);

            var request = Substitute.For<IGoalRequest>();
            request.Goals.Returns(new List<IGoal> { goal });

            this.goapActionProvider.CurrentPlan.Returns(goalResult);
            this.goapActionProvider.GoalRequest.Returns(request);
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal });

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseLocal(this.goapActionProvider, Arg.Any<IGoalRequest>());
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_RunsWithMultipleGoals()
        {
            // Arrange
            var goal1 = Substitute.For<IGoal>();
            goal1.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var goal2 = Substitute.For<IGoal>();
            goal2.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var goalRequest = Substitute.For<IGoalRequest>();
            goalRequest.Goals.Returns(new List<IGoal> { goal1, goal2 });

            this.goapActionProvider.GoalRequest.Returns(goalRequest);
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal1, goal2 });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal1, goal2 });

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseLocal(this.goapActionProvider, goalRequest);
            resolver.Received(1).StartResolve(Arg.Is<RunData>(x => x.StartIndex.Length == 2));
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndDoesHaveAction_Runs()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var goalResult = Substitute.For<IGoalResult>();
            goalResult.Goal.Returns(goal);

            var goalRequest = Substitute.For<IGoalRequest>();
            goalRequest.Goals.Returns(new List<IGoal> { goal });

            this.goapActionProvider.GoalRequest.Returns(goalRequest);
            this.goapActionProvider.CurrentPlan.Returns(goalResult);
            this.goapActionProvider.Receiver.ActionState.Action.Returns(Substitute.For<IAction>());

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal });

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseLocal(this.goapActionProvider, Arg.Any<IGoalRequest>());
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
        }

        [Test]
        public void Run_AgentHasCompletedGoal_CallsGoalCompleteEvent()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var goalResult = Substitute.For<IGoalResult>();
            goalResult.Goal.Returns(goal);

            var request = Substitute.For<IGoalRequest>();
            request.Goals.Returns(new List<IGoal> { goal });

            this.goapActionProvider.GoalRequest.Returns(request);
            this.goapActionProvider.CurrentPlan.Returns(goalResult);
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal });

            this.agentType.GoapConfig.ConditionObserver.IsMet(Arg.Any<ICondition>()).Returns(true);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseLocal(this.goapActionProvider, goal);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
            this.goapActionProvider.Events.Received(1).GoalCompleted(goal);
        }

        [Test]
        public void Run_AgentHasNotCompletedGoal_DoesntCallGoalCompleteEvent()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var goalResult = Substitute.For<IGoalResult>();
            goalResult.Goal.Returns(goal);

            var request = Substitute.For<IGoalRequest>();
            request.Goals.Returns(new List<IGoal> { goal });

            this.goapActionProvider.GoalRequest.Returns(request);
            this.goapActionProvider.CurrentPlan.Returns(goalResult);
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal });

            this.agentType.GoapConfig.ConditionObserver.IsMet(Arg.Any<ICondition>()).Returns(false);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseLocal(this.goapActionProvider, goal);
            resolver.Received(1).StartResolve(Arg.Any<RunData>());
            this.goapActionProvider.Events.Received(0).GoalCompleted(goal);
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndNoAction_SetsTheActionOnAgent()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var action = Substitute.For<IGoapAction>();

            var goalResult = Substitute.For<IGoalResult>();
            goalResult.Goal.Returns(goal);

            var goalRequest = Substitute.For<IGoalRequest>();
            goalRequest.Goals.Returns(new List<IGoal> { goal });

            this.goapActionProvider.GoalRequest.Returns(goalRequest);
            this.goapActionProvider.CurrentPlan.Returns(goalResult);
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal });

            var handle = Substitute.For<IResolveHandle>();
            handle.Complete().Returns(new JobResult
            {
                Goal = goal,
                Actions = new IConnectable[] { action },
            });

            var resolver = Substitute.For<IGraphResolver>();
            resolver.StartResolve(Arg.Any<RunData>()).Returns(handle);

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Complete();

            // Assert
            this.goapActionProvider.Received(1).SetAction(Arg.Any<IGoalResult>());
        }

        [Test]
        public void Run_AgentHasCurrentGoalAndAction_ResolvingSameActionDoesntCallSet()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var action = Substitute.For<IGoapAction>();

            var goalResult = Substitute.For<IGoalResult>();
            goalResult.Goal.Returns(goal);

            var goalRequest = Substitute.For<IGoalRequest>();
            goalRequest.Goals.Returns(new List<IGoal> { goal });

            this.goapActionProvider.GoalRequest.Returns(goalRequest);
            this.goapActionProvider.CurrentPlan.Returns(goalResult);
            this.goapActionProvider.Receiver.ActionState.Action.Returns(action);

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal });

            var handle = Substitute.For<IResolveHandle>();
            handle.Complete().Returns(new JobResult
            {
                Goal = goal,
                Actions = new IConnectable[] { action },
            });

            var resolver = Substitute.For<IGraphResolver>();
            resolver.StartResolve(Arg.Any<RunData>()).Returns(handle);

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Complete();

            // Assert
            this.goapActionProvider.Received(0).SetAction(Arg.Any<IGoalResult>());
        }

        [Test]
        public void Run_WithMultipleGoals_DoesntResolveCompletedGoal()
        {
            // Arrange
            var condition1 = Substitute.For<ICondition>();
            var goal1 = Substitute.For<IGoal>();
            goal1.Conditions.Returns(new[] { condition1 });

            var condition2 = Substitute.For<ICondition>();
            var goal2 = Substitute.For<IGoal>();
            goal2.Conditions.Returns(new[] { condition2 });

            var goalRequest = Substitute.For<IGoalRequest>();
            goalRequest.Goals.Returns(new List<IGoal>() { goal1, goal2 });

            this.goapActionProvider.GoalRequest.Returns(goalRequest);
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var goapConfig = Substitute.For<IGoapConfig>();
            goapConfig.ConditionObserver.IsMet(condition1).Returns(true);
            goapConfig.ConditionObserver.IsMet(condition2).Returns(false);

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal1, goal2 });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal1, goal2 });
            this.agentType.GoapConfig.Returns(goapConfig);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(1).SenseLocal(this.goapActionProvider, goalRequest);
            resolver.Received(1).StartResolve(Arg.Is<RunData>(x => x.StartIndex.Length == 1));
        }

        [Test]
        public void Run_AgentWithMisMatchingAgentTypes_DoesntRun()
        {
            // Arrange
            var goal = Substitute.For<IGoal>();
            goal.Conditions.Returns(new[] { Substitute.For<ICondition>() });

            var goalResult = Substitute.For<IGoalResult>();
            goalResult.Goal.Returns(goal);

            var request = Substitute.For<IGoalRequest>();
            request.Goals.Returns(new List<IGoal>() { goal });

            this.goapActionProvider.CurrentPlan.Returns(goalResult);
            this.goapActionProvider.GoalRequest.Returns(request);
            this.goapActionProvider.Receiver.ActionState.Action.ReturnsNull();

            var sensorRunner = Substitute.For<ISensorRunner>();

            this.agentType.SensorRunner.Returns(sensorRunner);
            this.agentType.GetAllNodes().Returns(new List<IConnectable> { goal });
            this.agentType.GetActions().Returns(new List<IGoapAction>());
            this.agentType.GetGoals().Returns(new List<IGoal>() { goal });

            var otherAgentType = Substitute.For<IAgentType>();
            this.goapActionProvider.AgentType.Returns(otherAgentType);

            var resolver = Substitute.For<IGraphResolver>();

            var runner = new AgentTypeJobRunner(this.agentType, resolver);

            // Act
            runner.Run(new[] { this.goapActionProvider });
            runner.Dispose();

            // Assert
            sensorRunner.Received(0).SenseLocal(this.goapActionProvider, Arg.Any<IGoalRequest>());
            resolver.Received(0).StartResolve(Arg.Any<RunData>());
        }
    }
}
