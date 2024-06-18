using System;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
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
            
            switch (this.agent.CurrentAction.Config.MoveMode)
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
            var isValid = this.agent.CurrentAction.IsValid(this.agent, this.agent.CurrentActionData);
            
            if (!isValid)
            {
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
            this.proxy.SetMoveState(AgentMoveState.OutOfRange);
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
            this.proxy.SetMoveState(AgentMoveState.OutOfRange);
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
            
            var state = this.agent.CurrentAction.Perform(this.agent, this.agent.CurrentActionData, this.context);
            
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
            if (this.agent.RunState == null)
                return true;
            
            if (this.agent.RunState.IsCompleted(this.agent))
            {
                this.agent.CompleteAction();
                return false;
            }
            
            if (this.agent.RunState.ShouldStop(this.agent))
            {
                this.agent.StopAction();
                return false;
            }

            return this.agent.RunState.ShouldPerform(this.agent);
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