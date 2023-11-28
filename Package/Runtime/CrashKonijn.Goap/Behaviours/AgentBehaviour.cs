using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Exceptions;
using CrashKonijn.Goap.Observers;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        [FormerlySerializedAs("goapSetBehaviour")] public AgentTypeBehaviour agentTypeBehaviour;

        [field: SerializeField]
        public float DistanceMultiplier { get; set; } = 1f;
        public AgentState State { get; private set; } = AgentState.NoAction;
        public AgentMoveState MoveState { get; set; } = AgentMoveState.Idle;

        private IAgentType agentType;
        public IAgentType AgentType
        {
            get => this.agentType;
            set
            {
                this.agentType = value;
                value.Register(this);
                this.Events.Bind(this, value.Events);
            }
        }

        public IGoal CurrentGoal { get; private set; }
        public IAction CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public List<IAction> CurrentActionPath { get; private set; } = new List<IAction>();
        public IWorldData WorldData { get; } = new LocalWorldData();
        public IAgentEvents Events { get; } = new AgentEvents();
        public IDataReferenceInjector Injector { get; private set; }
        public IAgentDistanceObserver DistanceObserver { get; set; } = new VectorDistanceObserver();
        
        public IAgentTimers Timers { get; } = new AgentTimers();

        private ITarget currentTarget;

        private void Awake()
        {
            this.Injector = new DataReferenceInjector(this);
            
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
            this.EndAction(false);
            
            if (this.AgentType != null)
                this.AgentType.Unregister(this);
        }

        public void Run()
        {
            if (this.CurrentAction == null)
            {
                this.State = AgentState.NoAction;
                return;
            }
            
            this.UpdateTarget();

            switch (this.CurrentAction.Config.MoveMode)
            {
                case ActionMoveMode.MoveBeforePerforming:
                    this.RunMoveBeforePerforming();
                    break;
                case ActionMoveMode.PerformWhileMoving:
                    this.RunPerformWhileMoving();
                    break;
            }
        }

        private void RunPerformWhileMoving()
        {
            if (this.IsInRange())
            {
                this.State = AgentState.PerformingAction;
                this.SetMoveState(AgentMoveState.InRange);
                this.PerformAction();
                return;
            }
                
            this.State = AgentState.MovingWhilePerformingAction;
            this.SetMoveState(AgentMoveState.OutOfRange);
            this.Move();
            this.PerformAction();
        }

        private void RunMoveBeforePerforming()
        {
            if (this.IsInRange())
            {
                this.State = AgentState.PerformingAction;
                this.SetMoveState(AgentMoveState.InRange);
                this.PerformAction();
                return;
            }

            this.State = AgentState.MovingToTarget;
            this.SetMoveState(AgentMoveState.OutOfRange);
            this.Move();
        }

        private void UpdateTarget()
        {
            if (this.currentTarget == this.CurrentActionData?.Target)
                return;
            
            this.currentTarget = this.CurrentActionData?.Target;
            this.Events.TargetChanged(this.currentTarget, this.IsInRange());
        }

        private void SetMoveState(AgentMoveState state)
        {
            if (this.MoveState == state)
                return;
            
            this.MoveState = state;
            
            switch (state)
            {
                case AgentMoveState.InRange:
                    this.Events.TargetInRange(this.currentTarget);
                    break;
                case AgentMoveState.OutOfRange:
                    this.Events.TargetOutOfRange(this.currentTarget);
                    break;
            }
        }

        private void Move()
        {
            if (this.currentTarget == null)
                return;
            
            this.Events.Move(this.currentTarget);
        }

        private void PerformAction()
        {
            var result = this.CurrentAction.Perform(this, this.CurrentActionData, new ActionContext
            {
                DeltaTime = Time.deltaTime,
            });

            if (result == ActionRunState.Continue)
                return;
            
            this.EndAction();
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
                this.EndAction();
        }

        public void SetAction(IAction action, List<IAction> path, ITarget target)
        {
            if (this.CurrentAction != null)
            {
                this.EndAction(false);
            }

            this.CurrentAction = action;
            this.Timers.Action.Touch();

            var data = action.GetData();
            this.Injector.Inject(data);
            this.CurrentActionData = data;
            this.CurrentActionData.Target = target;
            this.CurrentActionPath = path;
            this.CurrentAction.Start(this, this.CurrentActionData);
            this.Events.ActionStart(action);
        }
        
        public void EndAction(bool resolveAction = true)
        {
            var action = this.CurrentAction;
            
            this.CurrentAction?.End(this, this.CurrentActionData);
            this.CurrentAction = null;
            this.CurrentActionData = null;
            this.currentTarget = null;
            this.MoveState = AgentMoveState.Idle;
            
            this.Events.ActionStop(action);
            
            if (resolveAction)
                this.ResolveAction();
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
    }
}