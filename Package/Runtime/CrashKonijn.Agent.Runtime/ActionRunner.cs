using System;
using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public class ActionRunner
    {
        private readonly IMonoAgent agent;
        private readonly IAgentProxy proxy;
        private readonly ActionContext context = new();

        public ActionRunner(IMonoAgent agent, IAgentProxy proxy)
        {
            this.agent = agent;
            this.proxy = proxy;
        }

        public void Run()
        {
            if (!this.RunIsValid())
                return;

            switch (this.agent.ActionState.Action.GetMoveMode(this.agent))
            {
                case ActionMoveMode.MoveBeforePerforming:
                    this.RunMoveBeforePerforming();
                    break;
                case ActionMoveMode.PerformWhileMoving:
                    this.RunPerformWhileMoving();
                    break;
            }
        }

        private bool RunIsValid()
        {
            if (this.agent.ActionState?.Action == null)
            {
                this.agent.Logger.Warning("No action to run!");
                return false;
            }

            var isValid = this.agent.ActionState.Action.IsValid(this.agent, this.agent.ActionState.Data);

            if (!isValid)
            {
                this.agent.Logger.Warning($"Action {this.agent.ActionState.Action.GetType().GetGenericTypeName()} is not valid!");
                this.agent.StopAction();
                return false;
            }

            return true;
        }

        private void RunPerformWhileMoving()
        {
            if (this.proxy.IsInRange())
            {
                this.proxy.SetState(AgentState.PerformingAction);
                this.proxy.SetMoveState(AgentMoveState.InRange);
                this.PerformAction();
                return;
            }

            this.proxy.SetState(AgentState.MovingWhilePerformingAction);
            this.proxy.SetMoveState(AgentMoveState.NotInRange);
            this.Move();
            this.PerformAction();
        }

        private void RunMoveBeforePerforming()
        {
            if (this.proxy.IsInRange() || this.agent.State == AgentState.PerformingAction)
            {
                this.proxy.SetState(AgentState.PerformingAction);
                this.proxy.SetMoveState(AgentMoveState.InRange);
                this.PerformAction();
                return;
            }

            this.proxy.SetState(AgentState.MovingToTarget);
            this.proxy.SetMoveState(AgentMoveState.NotInRange);
            this.Move();
        }

        private void Move()
        {
            if (this.agent.CurrentTarget == null)
                return;

            this.agent.Events.Move(this.agent.CurrentTarget);
        }

        private void PerformAction()
        {
            if (!this.ShouldContinue())
                return;

            this.context.DeltaTime = Time.deltaTime;
            this.context.IsInRange = this.proxy.IsInRange();

            if (!this.agent.ActionState.HasPerformed)
                this.agent.ActionState.Action.BeforePerform(this.agent, this.agent.ActionState.Data);

            var state = this.agent.ActionState.Action.Perform(this.agent, this.agent.ActionState.Data, this.context);

            if (state.IsCompleted(this.agent))
            {
                this.agent.CompleteAction();
                return;
            }

            if (state.ShouldStop(this.agent))
            {
                this.agent.StopAction();
                return;
            }

            this.proxy.SetRunState(state);
        }

        private bool ShouldContinue()
        {
            if (this.agent.ActionState.RunState == null)
                return true;

            this.agent.ActionState.RunState.Update(this.agent, this.context);

            if (this.agent.ActionState.RunState.IsCompleted(this.agent))
            {
                this.agent.CompleteAction();
                return false;
            }

            if (this.agent.ActionState.RunState.ShouldStop(this.agent))
            {
                this.agent.StopAction();
                return false;
            }

            return this.agent.ActionState.RunState.ShouldPerform(this.agent);
        }
    }

    public class AgentProxy : IAgentProxy
    {
        private readonly Action<AgentState> setState;
        private readonly Action<AgentMoveState> setMoveState;
        private readonly Action<IActionRunState> setRunState;
        private readonly Func<bool> isInRange;

        public AgentProxy(Action<AgentState> setState, Action<AgentMoveState> setMoveState, Action<IActionRunState> setRunState, Func<bool> isInRange)
        {
            this.setState = setState;
            this.setMoveState = setMoveState;
            this.setRunState = setRunState;
            this.isInRange = isInRange;
        }

        public void SetState(AgentState state) => this.setState(state);
        public void SetMoveState(AgentMoveState state) => this.setMoveState(state);
        public void SetRunState(IActionRunState state) => this.setRunState(state);
        public bool IsInRange() => this.isInRange();
    }

    public interface IAgentProxy
    {
        void SetState(AgentState state);
        void SetMoveState(AgentMoveState state);
        void SetRunState(IActionRunState state);
        bool IsInRange();
    }
}
