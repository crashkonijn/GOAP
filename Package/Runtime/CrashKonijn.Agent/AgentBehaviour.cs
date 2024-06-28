using System;
using System.Collections.Generic;
using CrashKonijn.Agent;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Observers;
using UnityEngine;
using ILogger = CrashKonijn.Goap.Core.Interfaces.ILogger;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        [field: SerializeField]
        public ActionProviderBase ActionProvider { get; set; }
        
        [field: SerializeField]
        public LoggerConfig LoggerConfig { get; set; } = new LoggerConfig();
        
        public AgentState State { get; private set; } = AgentState.NoAction;
        public AgentMoveState MoveState { get; private set; } = AgentMoveState.Idle;
        public List<Type> DisabledActions { get; } = new ();
        
        [Obsolete("Use ActionState.Action instead.")]
        public IAction CurrentAction => this.ActionState.Action;
        [Obsolete("Use ActionState.Data instead.")]
        public IActionData CurrentActionData => this.ActionState.Data;
        [Obsolete("Use ActionState.RunState instead.")]
        public IActionRunState RunState => this.ActionState.RunState;
        public IActionState ActionState { get; } = new ActionState();
        
        public IAgentEvents Events { get; } = new AgentEvents();
        public IDataReferenceInjector Injector { get; private set; }
        public IAgentDistanceObserver DistanceObserver { get; set; } = new VectorDistanceObserver();
        public ILogger<IMonoAgent> Logger { get; set; } = new AgentLogger();
        public IActionResolver ActionResolver { get; set; }

        public IAgentTimers Timers { get; } = new AgentTimers();

        public ITarget CurrentTarget { get; private set; }
        
        public Transform Transform => this.transform;
        
        private ActionRunner actionRunner;

        private void Awake()
        {
            this.Initialize();
        }
        
        public void Initialize()
        {
            this.Injector = new DataReferenceInjector(this);
            this.actionRunner = new ActionRunner(this, new AgentProxy(this.SetState, this.SetMoveState, (state) => this.ActionState.RunState = state, this.IsInRange));
            this.Logger.Initialize(this.LoggerConfig, this);
        }

        private void Update()
        {
            this.Run();
        }

        public void Run()
        {
            if (this.ActionState.Action == null)
            {
                this.SetState(AgentState.NoAction);
                return;
            }
            
            this.UpdateTarget();

            this.actionRunner.Run();
        }
        
        private void UpdateTarget()
        {
            if (this.CurrentTarget == this.ActionState.Data?.Target)
                return;
            
            this.CurrentTarget = this.ActionState.Data?.Target;
            this.Events.TargetChanged(this.CurrentTarget, this.IsInRange());
        }

        private void SetState(AgentState state)
        {
            if (this.State == state)
                return;
            
            this.State = state;

            if (state is AgentState.PerformingAction or AgentState.MovingWhilePerformingAction)
            {
                this.Timers.Action.Touch();
            }
        }

        private void SetMoveState(AgentMoveState state)
        {
            if (this.MoveState == state)
                return;
            
            this.MoveState = state;
            
            switch (state)
            {
                case AgentMoveState.InRange:
                    this.Events.TargetInRange(this.CurrentTarget);
                    break;
                case AgentMoveState.NotInRange:
                    this.Events.TargetNotInRange(this.CurrentTarget);
                    break;
            }
        }

        private bool IsInRange()
        {
            var distance = this.DistanceObserver.GetDistance(this, this.ActionState.Data?.Target, this.Injector);
            
            return this.ActionState.Action.IsInRange(this, distance, this.ActionState.Data, this.Injector);
        }
        
        public void SetAction(IActionResolver actionResolver, IAction action, ITarget target)
        {
            this.ActionResolver = actionResolver;
            
            if (this.ActionState.Action != null)
            {
                this.StopAction(false);
            }
            
            var data = action.GetData();
            this.Injector.Inject(data);
            data.Target = target;

            this.ActionState.SetAction(action, data);
            this.Timers.Action.Touch();

            action.Start(this, data);
            
            this.Events.ActionStart(action);
        }
        
        public void StopAction(bool resolveAction = true)
        {
            var action = this.ActionState.Action;
            
            action?.Stop(this, this.ActionState.Data);
            this.ResetAction();
            
            this.Events.ActionStop(action);
            
            if (resolveAction)
                this.ResolveAction();
        }

        public void CompleteAction(bool resolveAction = true)
        {
            var action = this.ActionState.Action;
            
            action?.Complete(this, this.ActionState.Data);
            this.ResetAction();
            
            this.Events.ActionComplete(action);
            
            if (resolveAction)
                this.ResolveAction();
        }

        public void ResolveAction()
        {
            if (this.ActionResolver == null)
                throw new AgentException("No action resolver found!");
            
            this.ActionResolver.ResolveAction();
        }

        private void ResetAction()
        {
            this.ActionState.Reset();
            this.CurrentTarget = null;
            this.MoveState = AgentMoveState.Idle;
        }

        
        public void EnableAction<TAction>()
            where TAction : IAction
        {
            if (!this.DisabledActions.Contains(typeof(TAction)))
                return;
            
            this.DisabledActions.Remove(typeof(TAction));
        }
        
        public void DisableAction<TAction>()
            where TAction : IAction
        {
            if (this.DisabledActions.Contains(typeof(TAction)))
                return;
            
            this.DisabledActions.Add(typeof(TAction));
        }
    }
}