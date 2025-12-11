using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Runtime;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Unity.Collections;

namespace CrashKonijn.Goap.UnitTests
{
    [TestFixture]
    public class ActionRunnerTests
    {
        private IMonoAgent agent;
        private ActionRunner actionRunner;
        private IAgentProxy proxy;
        private IAgentEvents events;
        private IGoapAction action;

        [SetUp]
        public void SetUp()
        {
            this.events = Substitute.For<IAgentEvents>();
            this.action = Substitute.For<IGoapAction>();
            this.action.IsValid(Arg.Any<IMonoAgent>(), Arg.Any<IActionData>()).Returns(true);

            this.agent = Substitute.For<IMonoAgent>();
            this.agent.Events.Returns(this.events);
            this.agent.ActionState.Action.Returns(this.action);
            this.agent.ActionState.RunState.ReturnsNull();

            this.proxy = Substitute.For<IAgentProxy>();

            this.actionRunner = new ActionRunner(this.agent, this.proxy);
            
            // Unity sometimes thinks that a temporary job is leaking memory
            // This is not the case, so we ignore the message
            // This can trigger in any test, even the ones that don't use the Job system
            NativeLeakDetection.Mode = NativeLeakDetectionMode.Disabled;
        }

        [Test]
        public void Run_WhenRunIsNotValid_ShouldStopAction()
        {
            // Arrange
            this.action.IsValid(this.agent, this.agent.ActionState.Data).Returns(false);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.agent.Received().StopAction();
        }

        [Test]
        public void MoveBeforePerforming_WhenInRange_ShouldSetStateToPerformingAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.MoveBeforePerforming);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetState(AgentState.PerformingAction);
        }

        [Test]
        public void MoveBeforePerforming_WhenInRange_ShouldSetMoveStateToInRange()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.MoveBeforePerforming);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetMoveState(AgentMoveState.InRange);
        }

        [Test]
        public void MoveBeforePerforming_WhenInRange_ShouldPerformAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.MoveBeforePerforming);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.action.Received().Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>());
        }

        [Test]
        public void MoveBeforePerforming_WhenNotInRange_ShouldSetStateToMovingToTarget()
        {
            // Arrange
            this.proxy.IsInRange().Returns(false);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.MoveBeforePerforming);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetState(AgentState.MovingToTarget);
        }

        [Test]
        public void MoveBeforePerforming_WhenNotInRange_ShouldSetMoveStateToOutOfRange()
        {
            // Arrange
            this.proxy.IsInRange().Returns(false);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.MoveBeforePerforming);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetMoveState(AgentMoveState.NotInRange);
        }

        [Test]
        public void MoveBeforePerforming_WhenNotInRange_ShouldMove()
        {
            // Arrange
            this.proxy.IsInRange().Returns(false);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.MoveBeforePerforming);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.agent.Events.Received().Move(this.agent.CurrentTarget);
        }

        [Test]
        public void MoveBeforePerforming_WhenNotInRangeAndStateIsPerformingAction_ShouldNotSetStateToMovingToTarget()
        {
            // Arrange
            this.proxy.IsInRange().Returns(false);
            this.agent.State.Returns(AgentState.PerformingAction);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.MoveBeforePerforming);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.DidNotReceive().SetState(AgentState.MovingToTarget);
        }

        [Test]
        public void PerformWhileMoving_WhenInRange_ShouldSetStateToPerformingAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetState(AgentState.PerformingAction);
        }

        [Test]
        public void PerformWhileMoving_WhenInRange_ShouldSetMoveStateToInRange()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetMoveState(AgentMoveState.InRange);
        }

        [Test]
        public void PerformWhileMoving_WhenInRange_ShouldPerformAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.action.Received().Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>());
        }

        [Test]
        public void PerformWhileMoving_WhenNotInRange_ShouldSetStateToMovingWhilePerformingAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(false);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetState(AgentState.MovingWhilePerformingAction);
        }

        [Test]
        public void PerformWhileMoving_WhenNotInRange_ShouldSetMoveStateToOutOfRange()
        {
            // Arrange
            this.proxy.IsInRange().Returns(false);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.proxy.Received().SetMoveState(AgentMoveState.NotInRange);
        }

        [Test]
        public void PerformWhileMoving_WhenNotInRange_ShouldMove()
        {
            // Arrange
            this.proxy.IsInRange().Returns(false);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.agent.Events.Received().Move(this.agent.CurrentTarget);
        }

        [Test]
        public void PerformAction_NoRunState_ShouldPerformAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.action.Received().Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>());
        }

        [Test]
        public void PerformAction_IsCompletedRunState_ShouldNotPerformAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.agent.ActionState.RunState.Returns(ActionRunState.Completed);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.action.DidNotReceive().Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>());
            this.agent.Received().CompleteAction();
        }

        [Test]
        public void PerformAction_ShouldStopRunState_ShouldNotPerformAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.agent.ActionState.RunState.Returns(ActionRunState.Stop);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.action.DidNotReceive().Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>());
            this.agent.Received().StopAction();
        }

        [Test]
        public void PerformAction_ContinueRunState_ShouldPerformAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.agent.ActionState.RunState.Returns(ActionRunState.Continue);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.action.Received().Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>());
        }

        [Test]
        public void PerformAction_ReturnedCompleteState_ShouldCompleteAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.agent.ActionState.RunState.Returns(ActionRunState.Completed);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);
            this.action.Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>()).Returns(ActionRunState.Completed);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.agent.Received().CompleteAction();
        }

        [Test]
        public void PerformAction_ReturnedStopState_ShouldStopAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);
            this.agent.ActionState.RunState.Returns(ActionRunState.Stop);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);
            this.action.Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>()).Returns(ActionRunState.Stop);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.agent.Received().StopAction();
        }

        [Test]
        public void PerformAction_ReturnedContinueState_ShouldContinueAction()
        {
            // Arrange
            this.proxy.IsInRange().Returns(true);

            var state = ActionRunState.Continue;

            this.agent.ActionState.RunState.Returns(state);
            this.action.GetMoveMode(this.agent).Returns(ActionMoveMode.PerformWhileMoving);
            this.action.Perform(this.agent, this.agent.ActionState.Data, Arg.Any<IActionContext>()).Returns(ActionRunState.Continue);

            // Act
            this.actionRunner.Run(0f);

            // Assert
            this.agent.DidNotReceive().CompleteAction();
            this.agent.DidNotReceive().StopAction();
            this.proxy.Received().SetRunState(state);
        }
    }
}
