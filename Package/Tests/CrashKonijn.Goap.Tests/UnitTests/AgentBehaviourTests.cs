using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
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
        [Test]
        public void OnEnable_CallsRegister()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;

            // Act
            Action act = () =>
            {
                agent.CallOnEnable();
            };

            // Assert
            goapSet.Received(1).Register(agent);

            act();
            
            goapSet.Received(2).Register(agent);
        }
        
        [Test]
        public void OnDisable_CallsUnregister()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            
            // Act
            Action act = () =>
            {
                agent.CallOnDisable();
            };
            
            // Assert
            goapSet.Received(0).Unregister(agent);
            
            act();
            
            goapSet.Received(1).Unregister(agent);
        }
        
        [Test]
        public void Run_WithoutAction_SetsStateToNoAction()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            
            // Act
            agent.Run();
            
            // Assert
            agent.State.Should().Be(AgentState.NoAction);
        }

        [Test]
        public void Run_MoveBeforePerforming_WithActionAndTooMuchDistance_SetsStateToMovingToTarget()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();

            var action = Substitute.For<IActionBase>();
            action.Config.MoveMode.Returns(ActionMoveMode.MoveBeforePerforming);
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.up * 100f));
            
            // Act
            agent.Run();
            
            // Assert
            agent.State.Should().Be(AgentState.MovingToTarget);
        }

        [Test]
        public void Run_MoveBeforePerforming_WithActionAndTooMuchDistance_CallsMover()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();

            var action = Substitute.For<IActionBase>();
            action.Config.MoveMode.Returns(ActionMoveMode.MoveBeforePerforming);
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.up * 100f));
            agent.MockEvents();
            
            // Act
            agent.Run();
            
            // Assert
            agent.Events.Received(1).Move(Arg.Any<ITarget>());
            action.Received(0).Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>());
        }

        [Test]
        public void Run_MoveBeforePerforming_WithCloseAction_CallsActionPerform()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var action = Substitute.For<IActionBase>();
            action.Config.MoveMode.Returns(ActionMoveMode.MoveBeforePerforming);
            action.IsInRange(agent, Arg.Any<float>(), Arg.Any<IActionData>(), Arg.Any<IDataReferenceInjector>()).Returns(true);
            action.Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>()).Returns(ActionRunState.Continue);
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            agent.MockEvents();
            
            // Act
            agent.Run();
            
            // Assert
            agent.Events.Received(0).Move(Arg.Any<ITarget>());
            action.Received(1).Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>());
            action.Received(0).End(agent, Arg.Any<IActionData>());
        }

        [Test]
        public void Run_PerformWhileMoving_WithActionAndTooMuchDistance_SetsStateToMovingWhilePerformingAction()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var action = Substitute.For<IActionBase>();
            action.Config.MoveMode.Returns(ActionMoveMode.PerformWhileMoving);
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.up * 100f));
            
            // Act
            agent.Run();
            
            // Assert
            agent.State.Should().Be(AgentState.MovingWhilePerformingAction);
        }

        [Test]
        public void Run_PerformWhileMoving_WithActionAndTooMuchDistance_CallsMoverAndAction()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();

            var action = Substitute.For<IActionBase>();
            action.Config.MoveMode.Returns(ActionMoveMode.PerformWhileMoving);
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.up * 100f));
            agent.MockEvents();
            
            // Act
            agent.Run();
            
            // Assert
            agent.Events.Received(1).Move(Arg.Any<ITarget>());
            action.Received(1).Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>());
        }

        [Test]
        public void Run_PerformWhileMoving_WithCloseAction_CallsActionPerform()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();

            var action = Substitute.For<IActionBase>();
            action.Config.MoveMode.Returns(ActionMoveMode.PerformWhileMoving);
            action.IsInRange(agent, Arg.Any<float>(), Arg.Any<IActionData>(), Arg.Any<IDataReferenceInjector>()).Returns(true);
            action.Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>()).Returns(ActionRunState.Continue);
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            agent.MockEvents();
            
            // Act
            agent.Run();
            
            // Assert
            agent.Events.Received(0).Move(Arg.Any<ITarget>());
            action.Received(1).Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>());
            action.Received(0).End(agent, Arg.Any<IActionData>());
        }

        [Test]
        public void ActionPerform_WithRunStateStop_CallsEnd()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            agent.CallAwake();
            
            var action = Substitute.For<IActionBase>();
            action.IsInRange(agent, Arg.Any<float>(), Arg.Any<IActionData>(), Arg.Any<IDataReferenceInjector>()).Returns(true);
            action.Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>()).Returns(ActionRunState.Stop);
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            
            // Act
            agent.Run();
            
            // Assert
            action.Received(1).Perform(agent, Arg.Any<IActionData>(), Arg.Any<ActionContext>());
            action.Received(1).End(agent, Arg.Any<IActionData>());
        }

        [Test]
        public void SetGoal_SetsGoal()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.ResolveGoal<TestGoal>().Returns(new TestGoal());
            
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            
            // Act
            agent.SetGoal<TestGoal>(false);
            
            // Assert
            agent.CurrentGoal.Should().BeOfType<TestGoal>();
        }

        [Test]
        public void SetGoal_EnqueuesAgent()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.ResolveGoal<TestGoal>().Returns(new TestGoal());
            
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            
            // Act
            agent.SetGoal<TestGoal>(false);
            
            // Assert
            goapSet.Agents.Received(1).Enqueue(agent);
        }

        [Test]
        public void SetGoal_CallsGoalStartEvent()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.ResolveGoal<TestGoal>().Returns(new TestGoal());
            
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            agent.MockEvents();
            
            // Act
            agent.SetGoal<TestGoal>(false);
            
            // Assert
            agent.Events.Received(1).GoalStart(Arg.Any<IGoalBase>());
        }
        
        [Test]
        public void SetGoal_EndActionFalse_DoesntCallEnd()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.ResolveGoal<TestGoal>().Returns(new TestGoal());
            
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            
            // Set Action property through reflection
            var action = Substitute.For<IActionBase>();
            agent.InsertAction(action);
            
            // Act
            agent.SetGoal<TestGoal>(false);
            
            // Assert
            action.Received(0).End(Arg.Any<IMonoAgent>(), Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetGoal_EndActionTrue_DoesCallEnd()
        {
            // Arrange
            var goapSet = Substitute.For<IGoapSet>();
            goapSet.ResolveGoal<TestGoal>().Returns(new TestGoal());
            
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = goapSet;
            
            // Set Action property through reflection
            var action = Substitute.For<IActionBase>();
            agent.InsertAction(action);
            
            // Act
            agent.SetGoal<TestGoal>(true);
            
            // Assert
            action.Received(1).End(Arg.Any<IMonoAgent>(), Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetAction_SetsAction()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var action = Substitute.For<IActionBase>();
            
            // Act
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            
            // Assert
            agent.CurrentAction.Should().Be(action);
        }
        
        [Test]
        public void SetAction_CallsEndOnOldAction()
        {
            // Arrange
            var set = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.GoapSet = set;
            agent.CallAwake();

            var action = Substitute.For<IActionBase>();
            
            // Set Action property through reflection
            var oldAction = Substitute.For<IActionBase>();
            agent.InsertAction(oldAction);
            
            // Act
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            
            // Assert
            oldAction.Received(1).End(agent, Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetAction_CallsGetData()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var action = Substitute.For<IActionBase>();
            
            // Act
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            
            // Assert
            action.Received(1).GetData();
        }
        
        [Test]
        public void SetAction_StoresData()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var actionData = Substitute.For<IActionData>();
            var action = Substitute.For<IActionBase>();
            action.GetData().Returns(actionData);
            
            // Act
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            
            // Assert
            agent.CurrentActionData.Should().Be(actionData);
        }
        
        [Test]
        public void SetAction_SetsDataTarget()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var actionData = Substitute.For<IActionData>();
            var action = Substitute.For<IActionBase>();
            action.GetData().Returns(actionData);

            var target = new PositionTarget(Vector3.zero);
            
            // Act
            agent.SetAction(action, new List<IActionBase>(), target);
            
            // Assert
            actionData.Target.Should().Be(target);
        }
        
        [Test]
        public void SetAction_CallsStartOnAction()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var action = Substitute.For<IActionBase>();

            // Act
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            
            // Assert
            action.Received(1).Start(agent, Arg.Any<IActionData>());
        }
        
        [Test]
        public void SetAction_StoresPath()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            
            var action = Substitute.For<IActionBase>();
            var path = new List<IActionBase>
            {
                Substitute.For<IActionBase>()
            };
            
            // Act
            agent.SetAction(action, path, new PositionTarget(Vector3.zero));
            
            // Assert
            agent.CurrentActionPath.Should().BeSameAs(path);
        }
        
        [Test]
        public void SetAction_CallsActionStartEvent()
        {
            // Arrange
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            agent.MockEvents();

            var action = Substitute.For<IActionBase>();
            
            // Act
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));
            
            // Assert
            agent.Events.Received(1).ActionStart(action);
        }
        
        [Test]
        public void EndAction_CallsEndOnAction()
        {
            // Arrange
            var set = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            agent.GoapSet = set;
            
            var action = Substitute.For<IActionBase>();
            agent.InsertAction(action);
            
            // Act
            agent.EndAction();
            
            // Assert
            action.Received(1).End(agent, Arg.Any<IActionData>());
        }
        
        [Test]
        public void EndAction_ClearsAction()
        {
            // Arrange
            var set = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            agent.GoapSet = set;

            // Act
            agent.SetAction(Substitute.For<IActionBase>(), new List<IActionBase>(), new PositionTarget(Vector3.zero));
            agent.EndAction();
            
            // Assert
            agent.CurrentAction.Should().BeNull();
        }
        
        [Test]
        public void EndAction_ClearsActionData()
        {
            // Arrange
            var set = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            agent.GoapSet = set;

            // Act
            agent.SetAction(Substitute.For<IActionBase>(), new List<IActionBase>(), new PositionTarget(Vector3.zero));
            agent.EndAction();
            
            // Assert
            agent.CurrentActionData.Should().BeNull();
        }

        [Test]
        public void EndAction_ShouldEnqueueAgent()
        {
            // Arrange
            var set = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            agent.GoapSet = set;
            
            // Act
            agent.EndAction();
            
            // Assert
            set.Agents.Received(1).Enqueue(agent);
        }
        
        [Test]
        public void EndAction_CallsActionEndEvent()
        {
            // Arrange
            var set = Substitute.For<IGoapSet>();
            var agent = new GameObject("Agent").AddComponent<AgentBehaviour>();
            agent.CallAwake();
            agent.GoapSet = set;
            agent.MockEvents();

            var action = Substitute.For<IActionBase>();
            agent.SetAction(action, new List<IActionBase>(), new PositionTarget(Vector3.zero));

            // Act
            agent.EndAction();
            
            // Assert
            agent.Events.Received(1).ActionStop(action);
        }
    }
}