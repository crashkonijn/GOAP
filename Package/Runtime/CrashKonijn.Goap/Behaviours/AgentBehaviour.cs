using System;
using System.Collections.Generic;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Exceptions;
using CrashKonijn.Goap.Observers;
using UnityEngine;
using UnityEngine.Serialization;
using ILogger = CrashKonijn.Goap.Core.Interfaces.ILogger;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        [FormerlySerializedAs("goapSetBehaviour")] public AgentTypeBehaviour agentTypeBehaviour;

        [field: SerializeField]
        public float DistanceMultiplier { get; set; } = 1f;
        [field: SerializeField]
        public DebugMode DebugMode { get; set; } = DebugMode.None;
        [field: SerializeField]
        public int MaxLogSize { get; set; } = 20;
        public AgentState State { get; private set; } = AgentState.NoAction;
        public AgentMoveState MoveState { get; private set; } = AgentMoveState.Idle;
        public List<IAction> DisabledActions { get; } = new List<IAction>();

        private IAgentType agentType;
        public IAgentType AgentType
        {
            get => this.agentType;
            set
            {
                this.agentType = value;
                this.WorldData.SetParent(value.WorldData);
                value.Register(this);
                this.Events.Bind(this, value.Events);
            }
        }

        public IGoal CurrentGoal { get; private set; }
        public IAction CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public IConnectable[] CurrentPlan { get; private set; } = Array.Empty<IConnectable>();
        public ILocalWorldData WorldData { get; } = new LocalWorldData();
        public IAgentEvents Events { get; } = new AgentEvents();
        public IDataReferenceInjector Injector { get; private set; }
        public IAgentDistanceObserver DistanceObserver { get; set; } = new VectorDistanceObserver();
        public ILogger Logger { get; set; } = new Classes.Logger();
        
        public IAgentTimers Timers { get; } = new AgentTimers();
        public IActionRunState RunState { get; private set; }

        public ITarget CurrentTarget { get; private set; }
        private ActionRunner actionRunner;

        private void Awake()
        {
            this.Injector = new DataReferenceInjector(this);
            this.actionRunner = new ActionRunner(this, new AgentProxy(this.SetState, this.SetMoveState, (state) => this.RunState = state, this.IsInRange));
            this.Logger.Agent = this;
            
            if (this.agentTypeBehaviour != null)
                this.AgentType = this.agentTypeBehaviour.AgentType;
        }

        private void Start()
        {
            if (this.AgentType == null)
                throw new GoapException($"There is no AgentType assigned to the agent '{this.name}'! Please assign one in the inspector or through code in the Awake method.");
        }

        private void OnEnable()
        {
            if (this.AgentType != null)
                this.AgentType.Register(this);
        }

        private void OnDisable()
        {
            this.StopAction(false);
            
            if (this.AgentType != null)
                this.AgentType.Unregister(this);
        }

        public void Run()
        {
            if (this.CurrentAction == null)
            {
                this.SetState(AgentState.NoAction);
                return;
            }
            
            this.UpdateTarget();

            this.actionRunner.Run();
        }
        

        private void UpdateTarget()
        {
            if (this.CurrentTarget == this.CurrentActionData?.Target)
                return;
            
            this.CurrentTarget = this.CurrentActionData?.Target;
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
            var distance = this.DistanceObserver.GetDistance(this, this.CurrentActionData?.Target, this.Injector);
            
            return this.CurrentAction.IsInRange(this, distance, this.CurrentActionData, this.Injector);
        }

        public void SetGoal<TGoal>(bool endAction)
            where TGoal : IGoal
        {
            this.SetGoal(this.AgentType.ResolveGoal<TGoal>(), endAction);
        }

        public void SetGoal(IGoal goal, bool endAction)
        {
            if (goal == this.CurrentGoal)
                return;
            
            this.CurrentGoal = goal;
            this.Timers.Goal.Touch();
            
            if (this.CurrentAction == null)
                this.ResolveAction();
            
            this.Events.GoalStart(goal);
            
            if (endAction)
                this.StopAction();
        }

        public void ClearGoal()
        {
            this.CurrentGoal = null;
        }

        public void SetAction(IAction action, IConnectable[] path, ITarget target)
        {
            if (this.CurrentAction != null)
            {
                this.StopAction(false);
            }

            this.CurrentAction = action;
            this.Timers.Action.Touch();

            var data = action.GetData();
            this.Injector.Inject(data);
            this.CurrentActionData = data;
            this.CurrentActionData.Target = target;
            this.CurrentPlan = path;
            this.CurrentAction.Start(this, this.CurrentActionData);
            this.RunState = null;
            
            this.Events.ActionStart(action);
        }
        
        public void StopAction(bool resolveAction = true)
        {
            var action = this.CurrentAction;
            
            action?.Stop(this, this.CurrentActionData);
            this.ResetAction();
            
            this.Events.ActionStop(action);
            
            if (resolveAction)
                this.ResolveAction();
        }

        public void CompleteAction(bool resolveAction = true)
        {
            var action = this.CurrentAction;
            
            action?.Complete(this, this.CurrentActionData);
            this.ResetAction();
            
            this.Events.ActionComplete(action);
            
            if (resolveAction)
                this.ResolveAction();
        }

        private void ResetAction()
        {
            this.CurrentAction = null;
            this.CurrentActionData = null;
            this.CurrentTarget = null;
            this.MoveState = AgentMoveState.Idle;
            this.RunState = null;
        }

        public void SetDistanceMultiplierSpeed(float speed)
        {
            this.DistanceMultiplier = 1f / speed;
        }

        public void ResolveAction()
        {
            this.Events.Resolve();
            this.Timers.Resolve.Touch();
        }
        
        public void EnableAction<TAction>()
            where TAction : IAction
        {
            var action = this.AgentType.ResolveAction<TAction>();
            
            if (!this.DisabledActions.Contains(action))
                return;
            
            this.DisabledActions.Remove(action);
        }
        
        public void DisableAction<TAction>()
            where TAction : IAction
        {
            var action = this.AgentType.ResolveAction<TAction>();
            
            if (this.DisabledActions.Contains(action))
                return;
            
            this.DisabledActions.Add(action);
        }
    }
}