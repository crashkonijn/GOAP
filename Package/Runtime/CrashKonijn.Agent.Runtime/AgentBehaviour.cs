using System;
using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        [field: SerializeField]
        public ActionProviderBase ActionProviderBase { get; set; }

        private IActionProvider actionProvider;

        public IActionProvider ActionProvider
        {
            get => this.actionProvider;
            set
            {
                if (this.actionProvider == value)
                    return;

                this.actionProvider = value;
                this.actionProvider.Receiver = this;
            }
        }

        [field: SerializeField]
        public LoggerConfig LoggerConfig { get; set; } = new();

        public AgentState State { get; private set; } = AgentState.NoAction;
        public AgentMoveState MoveState { get; private set; } = AgentMoveState.Idle;

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

        public IAgentTimers Timers { get; } = new AgentTimers();

        public ITarget CurrentTarget { get; private set; }

        public Transform Transform => this.transform;

        private ActionRunner actionRunner;

        #region Obsolete Methods

        [Obsolete("Use GoapActionProvider.CurrentPlan.Goal instead")]
        public object CurrentGoal { get; set; }

        [Obsolete("Use GoapActionProvider.AgentType instead")]
        public object GoapSet { get; set; }

        #endregion

        private void Awake()
        {
            this.Initialize();
        }

        private void Start()
        {
            if (this.ActionProvider == null)
                throw new AgentException($"There is no ActionProvider assigned to the agent '{this.name}'! Please assign one in the inspector or through code in the Awake method.");
        }

        private void OnEnable()
        {
            if (this.ActionState.PreviousAction != null)
                this.ActionProvider?.ResolveAction();
        }

        private void OnDisable()
        {
            this.StopAction(false);
        }

        public void Initialize()
        {
            this.Injector = new DataReferenceInjector(this);
            this.actionRunner = new ActionRunner(this, new AgentProxy(this.SetState, this.SetMoveState, (state) => this.ActionState.RunState = state, this.IsInRange));
            this.Logger.Initialize(this.LoggerConfig, this);

            if (this.ActionProviderBase != null)
                this.ActionProvider = this.ActionProviderBase;
        }

        private void Update()
        {
            this.Run();
        }

        public void Run()
        {
            if (this.ActionState.Action == null)
                return;

            this.UpdateTarget();

            this.actionRunner.Run();
        }

        private void UpdateTarget()
        {
            if (this.CurrentTarget == this.ActionState.Data?.Target)
                return;

            this.CurrentTarget = this.ActionState.Data?.Target;

            if (this.CurrentTarget == null)
            {
                this.Events.TargetLost();
                return;
            }

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

        public void SetAction(IActionProvider actionProvider, IAction action, ITarget target)
        {
            this.ActionProvider = actionProvider;

            if (this.ActionState.Action != null)
            {
                this.StopAction(false);
            }

            var data = action.GetData();
            this.Injector.Inject(data);
            data.Target = target;

            this.ActionState.SetAction(action, data);
            this.Timers.Action.Touch();

            this.SetState(AgentState.StartingAction);

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
            if (this.ActionProvider == null)
                throw new AgentException("No action provider found!");

            this.ActionProvider.ResolveAction();
        }

        private void ResetAction()
        {
            this.ActionState.Reset();
            this.SetState(AgentState.NoAction);
            this.SetMoveState(AgentMoveState.Idle);
            this.UpdateTarget();
        }

        [Obsolete("Enable actions from within the action itself, or disable using actionProvider.GetActions<TAction>.ForEach((action) => action.Enable(IActionDisabler))")]
        public void EnableAction<TAction>()
            where TAction : IAction
        {
            throw new Exception("Enable actions from within the action itself, or disable using actionProvider.GetActions<TAction>.ForEach((action) => action.Enable(IActionDisabler))");
        }

        [Obsolete("Disable actions from within the action itself, or disable using actionProvider.GetActions<TAction>.ForEach((action) => action.Disable(IActionDisabler))")]
        public void DisableAction<TAction>()
            where TAction : IAction
        {
            throw new Exception("Disable actions from within the action itself, or disable using actionProvider.GetActions<TAction>.ForEach((action) => action.Disable(IActionDisabler))");
        }
    }
}
