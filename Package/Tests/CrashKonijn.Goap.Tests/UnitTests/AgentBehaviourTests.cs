using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using CrashKonijn.Goap.UnitTests.Classes;
using CrashKonijn.Goap.UnitTests.Support;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

namespace CrashKonijn.Goap.UnitTests
{
    public class AgentBehaviourTests
    {
        private IAgentType agentType;
        private AgentBehaviour agent;
        private GoapActionProvider provider;

        [SetUp]
        public void Setup()
        {
            this.agentType = Substitute.For<IAgentType>();

            var go = new GameObject("Agent");

            this.agent = go.AddComponent<AgentBehaviour>();

            this.provider = go.AddComponent<GoapActionProvider>();
            this.provider.AgentType = this.agentType;
            this.provider.Receiver = this.agent;

            this.agent.ActionProvider = this.provider;
            
            // Unity sometimes thinks that a temporary job is leaking memory
            // This is not the case, so we ignore the message
            // This can trigger in any test, even the ones that don't use the Job system
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }

        [Test]
        public void OnEnable_CallsRegister()
        {
            // Arrange

            // Act
            Action act = () =>
            {
                this.provider.CallOnEnable();
            };

            // Assert
            this.agentType.Received(1).Register(this.provider);

            act();

            this.agentType.Received(2).Register(this.provider);
        }

        [Test]
        public void OnDisable_CallsUnregister()
        {
            // Arrange

            // Act
            Action act = () =>
            {
                this.provider.CallOnDisable();
            };

            // Assert
            this.agentType.Received(0).Unregister(this.provider);

            act();

            this.agentType.Received(1).Unregister(this.provider);
        }

        [Test]
        public void ActionPerform_WithRunStateStop_CallsEnd()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var goal = Substitute.For<IGoal>();

            var action = Substitute.For<IGoapAction>();
            action.IsValid(Arg.Any<IMonoAgent>(), Arg.Any<IActionData>()).Returns(true);
            action.IsInRange(this.agent, Arg.Any<float>(), Arg.Any<IActionData>(), Arg.Any<IDataReferenceInjector>()).Returns(true);
            action.Perform(this.agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>()).Returns(ActionRunState.Stop);

            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Act
            this.agent.Run();

            // Assert
            action.Received(1).Perform(this.agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>());
            action.Received(1).Stop(this.agent, Arg.Any<IActionData>());
        }

        [Test]
        public void RequestGoal_SetsGoalRequest()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());

            this.provider.AgentType = this.agentType;

            // Act
            this.provider.RequestGoal<TestGoal>(false);

            // Assert
            this.provider.GoalRequest.Goals.Should().ContainItemsAssignableTo<TestGoal>();
        }

        [Test]
        public void RequestGoal_ResolveFalse_DoesntCallResolve()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());

            // Set Action property through reflection
            var action = Substitute.For<IGoapAction>();
            this.provider.MockEvents();
            this.provider.InsertAction(action);

            // Act
            this.provider.RequestGoal<TestGoal>(false);

            // Assert
            this.provider.Events.Received(0).Resolve();
        }

        [Test]
        public void RequestGoal_ResolveTrue_DoesCallResolve()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());

            this.agent.ActionProvider = Substitute.For<IActionProvider>();

            // Set Action property through reflection
            var action = Substitute.For<IGoapAction>();
            this.provider.MockEvents();
            this.provider.InsertAction(action);

            // Act
            this.provider.RequestGoal<TestGoal>(true);

            // Assert
            this.provider.Events.Received(1).Resolve();
        }

        [Test]
        public void SetAction_SetsAction()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var goal = Substitute.For<IGoal>();
            var action = Substitute.For<IGoapAction>();

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Assert
            this.provider.Receiver.ActionState.Action.Should().Be(action);
        }

        [Test]
        public void SetAction_CallsEndOnOldAction()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var goal = Substitute.For<IGoal>();
            var action = Substitute.For<IGoapAction>();

            // Set Action property through reflection
            var oldAction = Substitute.For<IGoapAction>();
            this.provider.InsertAction(oldAction);

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Assert
            oldAction.Received(1).Stop(this.agent, Arg.Any<IActionData>());
        }

        [Test]
        public void SetAction_CallsGetData()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var goal = Substitute.For<IGoal>();
            var action = Substitute.For<IGoapAction>();

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Assert
            action.Received(1).GetData();
        }

        [Test]
        public void SetAction_StoresData()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var actionData = Substitute.For<IActionData>();
            var goal = Substitute.For<IGoal>();

            var action = Substitute.For<IGoapAction>();
            action.GetData().Returns(actionData);

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Assert
            this.provider.Receiver.ActionState.Data.Should().Be(actionData);
        }

        [Test]
        public void SetAction_CallsStartOnAction()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var goal = Substitute.For<IGoal>();
            var action = Substitute.For<IGoapAction>();

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Assert
            action.Received(1).Start(this.agent, Arg.Any<IActionData>());
        }

        [Test]
        public void SetAction_StoresPath()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var goal = Substitute.For<IGoal>();
            var action = Substitute.For<IGoapAction>();
            var path = new IConnectable[]
            {
                Substitute.For<IGoapAction>(),
            };

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = path,
                Action = action,
            });

            // Assert
            this.provider.CurrentPlan.Plan.Should().BeSameAs(path);
        }

        [Test]
        public void SetAction_CallsActionStartEvent()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();
            this.agent.MockEvents();

            var goal = Substitute.For<IGoal>();
            var action = Substitute.For<IGoapAction>();

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Assert
            this.agent.Events.Received(1).ActionStart(action);
        }

        [Test]
        public void EndAction_CallsEndOnAction()
        {
            // Arrange
            this.agent.ActionProvider = Substitute.For<IActionProvider>();
            this.provider.CallAwake();
            this.agent.Initialize();

            var action = Substitute.For<IGoapAction>();
            this.provider.InsertAction(action);

            // Act
            this.agent.StopAction();

            // Assert
            action.Received(1).Stop(this.agent, Arg.Any<IActionData>());
        }

        [Test]
        public void EndAction_ClearsAction()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = Substitute.For<IGoal>(),
                Plan = Array.Empty<IConnectable>(),
                Action = Substitute.For<IGoapAction>(),
            });

            this.agent.StopAction();

            // Assert
            this.provider.Receiver.ActionState.Action.Should().BeNull();
        }

        [Test]
        public void EndAction_ClearsActionData()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            // Act
            this.provider.SetAction(new GoalResult
            {
                Goal = Substitute.For<IGoal>(),
                Plan = Array.Empty<IConnectable>(),
                Action = Substitute.For<IGoapAction>(),
            });
            this.agent.StopAction();

            // Assert
            this.provider.Receiver.ActionState.Data.Should().BeNull();
        }

        [Test]
        public void EndAction_ShouldResolveAgent()
        {
            // Arrange
            this.agent.ActionProvider = this.provider;
            this.provider.CallAwake();
            this.agent.Initialize();
            this.provider.MockEvents();

            // Act
            this.agent.StopAction();

            // Assert
            this.provider.Events.Received(1).Resolve();
        }

        [Test]
        public void EndAction_CallsActionEndEvent()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();
            this.agent.MockEvents();

            var goal = Substitute.For<IGoal>();
            var action = Substitute.For<IGoapAction>();
            this.provider.SetAction(new GoalResult
            {
                Goal = goal,
                Plan = Array.Empty<IConnectable>(),
                Action = action,
            });

            // Act
            this.agent.StopAction();

            // Assert
            this.agent.Events.Received(1).ActionStop(action);
        }

        [Test]
        public void Pause_CallsPauseEvent()
        {
            // Arrange
            this.agent.Initialize();
            this.agent.MockEvents();
            
            // Act
            this.agent.IsPaused = true;
            
            // Assert
            this.agent.Events.Received(1).Pause();
        }

        [Test]
        public void Pause_CallsUnpauseEvent()
        {
            // Arrange
            this.agent.Initialize();
            this.agent.MockEvents();
            this.agent.IsPaused = true;
            
            // Act
            this.agent.Events.Received(0).Resume();
            this.agent.IsPaused = false;
            
            // Assert
            this.agent.Events.Received(1).Resume();
        }
    }
}
