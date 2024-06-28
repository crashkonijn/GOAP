using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.RunStates;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.UnitTests.Classes;
using CrashKonijn.Goap.UnitTests.Support;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
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
            
            var action = Substitute.For<IGoapAction>();
            action.IsValid(Arg.Any<IMonoAgent>(), Arg.Any<IActionData>()).Returns(true);
            action.IsInRange(this.agent, Arg.Any<float>(), Arg.Any<IActionData>(), Arg.Any<IDataReferenceInjector>()).Returns(true);
            action.Perform(this.agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>()).Returns(ActionRunState.Stop);

            this.provider.SetAction(action, Array.Empty<IConnectable>());
            
            // Act
            this.agent.Run();
            
            // Assert
            action.Received(1).Perform(this.agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>());
            action.Received(1).Stop(this.agent, Arg.Any<IActionData>());
        }

        [Test]
        public void SetGoal_SetsGoal()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());

            this.provider.AgentType = this.agentType;
            
            // Act
            this.provider.SetGoal<TestGoal>(false);
            
            // Assert
            this.provider.CurrentGoal.Should().BeOfType<TestGoal>();
        }

        [Test]
        public void SetGoal_ResolvesAgent()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());

            this.provider.MockEvents();
            
            // Act
            this.provider.SetGoal<TestGoal>(false);
            
            // Assert
            this.provider.Events.Received(1).Resolve();
        }

        [Test]
        public void SetGoal_CallsGoalStartEvent()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());

            this.provider.MockEvents();
            
            // Act
            this.provider.SetGoal<TestGoal>(false);
            
            // Assert
            this.provider.Events.Received(1).GoalStart(Arg.Any<IGoal>());
        }
        
        [Test]
        public void SetGoal_EndActionFalse_DoesntCallEnd()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());
            
            // Set Action property through reflection
            var action = Substitute.For<IGoapAction>();
            this.provider.InsertAction(action);
            
            // Act
            this.provider.SetGoal<TestGoal>(false);
            
            // Assert
            action.Received(0).Stop(Arg.Any<IMonoAgent>(), Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetGoal_EndActionTrue_DoesCallEnd()
        {
            // Arrange
            this.agentType.ResolveGoal<TestGoal>().Returns(new TestGoal());

            this.agent.ActionResolver = Substitute.For<IActionResolver>();
            
            // Set Action property through reflection
            var action = Substitute.For<IGoapAction>();
            this.provider.InsertAction(action);
            
            // Act
            this.provider.SetGoal<TestGoal>(true);
            
            // Assert
            action.Received(1).Stop(Arg.Any<IMonoAgent>(), Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetAction_SetsAction()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();
            
            var action = Substitute.For<IGoapAction>();
            
            // Act
            this.provider.SetAction(action, Array.Empty<IConnectable>());
            
            // Assert
            this.provider.Agent.ActionState.Action.Should().Be(action);
        }
        
        [Test]
        public void SetAction_CallsEndOnOldAction()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            var action = Substitute.For<IGoapAction>();
            
            // Set Action property through reflection
            var oldAction = Substitute.For<IGoapAction>();
            this.provider.InsertAction(oldAction);
            
            // Act
            this.provider.SetAction(action, Array.Empty<IConnectable>());
            
            // Assert
            oldAction.Received(1).Stop(this.agent, Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetAction_CallsGetData()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();
            
            var action = Substitute.For<IGoapAction>();
            
            // Act
            this.provider.SetAction(action, Array.Empty<IConnectable>());
            
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
            var action = Substitute.For<IGoapAction>();
            action.GetData().Returns(actionData);
            
            // Act
            this.provider.SetAction(action, Array.Empty<IConnectable>());
            
            // Assert
            this.provider.Agent.ActionState.Data.Should().Be(actionData);
        }
        
        // TODO
        // [Test]
        // public void SetAction_SetsDataTarget()
        // {
        //     // Arrange
        //     var agent = new GameObject("Agent").AddComponent<GoapAgentBehaviour>();
        //     agent.CallAwake();
        //     agent.Agent.Initialize();
        //     
        //     var actionData = Substitute.For<IActionData>();
        //     var action = Substitute.For<IGoapAction>();
        //     action.GetData().Returns(actionData);
        //
        //     var target = new PositionTarget(Vector3.zero);
        //     
        //     // Act
        //     agent.SetAction(action, Array.Empty<IConnectable>(), target);
        //     
        //     // Assert
        //     actionData.Target.Should().Be(target);
        // }
        
        [Test]
        public void SetAction_CallsStartOnAction()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();
            
            var action = Substitute.For<IGoapAction>();

            // Act
            this.provider.SetAction(action, Array.Empty<IConnectable>());
            
            // Assert
            action.Received(1).Start(this.agent, Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetAction_StoresPath()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();
            
            var action = Substitute.For<IGoapAction>();
            var path = new IConnectable[]
            {
                Substitute.For<IGoapAction>()
            };
            
            // Act
            this.provider.SetAction(action, path);
            
            // Assert
            this.provider.CurrentPlan.Should().BeSameAs(path);
        }
        
        [Test]
        public void SetAction_CallsActionStartEvent()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();
            this.agent.MockEvents();

            var action = Substitute.For<IGoapAction>();
            
            // Act
            this.provider.SetAction(action, Array.Empty<IConnectable>());
            
            // Assert
            this.agent.Events.Received(1).ActionStart(action);
        }
        
        [Test]
        public void EndAction_CallsEndOnAction()
        {
            // Arrange
            this.agent.ActionResolver = Substitute.For<IActionResolver>();
            this.provider.CallAwake();
            this.agent.Initialize();
            
            var action = Substitute.For<IGoapAction>();
            this.provider.InsertAction(action);
            
            // Act
            this.provider.StopAction();
            
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
            this.provider.SetAction(Substitute.For<IGoapAction>(), Array.Empty<IConnectable>());
            this.provider.StopAction();
            
            // Assert
            this.provider.Agent.ActionState.Action.Should().BeNull();
        }
        
        [Test]
        public void EndAction_ClearsActionData()
        {
            // Arrange
            this.provider.CallAwake();
            this.agent.Initialize();

            // Act
            this.provider.SetAction(Substitute.For<IGoapAction>(), Array.Empty<IConnectable>());
            this.provider.StopAction();
            
            // Assert
            this.provider.Agent.ActionState.Data.Should().BeNull();
        }

        [Test]
        public void EndAction_ShouldResolveAgent()
        {
            // Arrange
            this.agent.ActionResolver = this.provider;
            this.provider.CallAwake();
            this.agent.Initialize();
            this.provider.MockEvents();
            
            // Act
            this.provider.StopAction();
            
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

            var action = Substitute.For<IGoapAction>();
            this.provider.SetAction(action, Array.Empty<IConnectable>());

            // Act
            this.provider.StopAction();
            
            // Assert
            this.agent.Events.Received(1).ActionStop(action);
        }
    }
}