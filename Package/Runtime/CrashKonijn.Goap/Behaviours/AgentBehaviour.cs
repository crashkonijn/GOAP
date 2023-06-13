using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Exceptions;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Observers;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        public GoapSetBehaviour goapSetBehaviour;

        [field: SerializeField]
        public float DistanceMultiplier { get; set; } = 1f;
        public AgentState State { get; private set; } = AgentState.NoAction;
        public AgentMoveState MoveState { get; set; } = AgentMoveState.Idle;

        private IGoapSet goapSet;
        public IGoapSet GoapSet
        {
            get => this.goapSet;
            set
            {
                this.goapSet = value;
                value.Register(this);
            }
        }

        public IGoalBase CurrentGoal { get; private set; }
        public IActionBase CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public List<IActionBase> CurrentActionPath { get; private set; } = new List<IActionBase>();
        public IWorldData WorldData { get; } = new LocalWorldData();
        public IAgentEvents Events { get; } = new AgentEvents();
        public IDataReferenceInjector Injector { get; private set; }
        public IAgentDistanceObserver DistanceObserver { get; set; } = new VectorDistanceObserver();

        private ITarget currentTarget;

        private void Awake()
        {
            this.Injector = new DataReferenceInjector(this);
            
            if (this.goapSetBehaviour != null)
                this.GoapSet = this.goapSetBehaviour.GoapSet;
        }

        private void Start()
        {
            if (this.GoapSet == null)
                throw new GoapException($"There is no GoapSet assigned to the agent '{this.name}'! Please assign one in the inspector or through code in the Awake method.");
        }

        private void OnEnable()
        {
            if (this.GoapSet != null)
                this.GoapSet.Register(this);
        }

        private void OnDisable()
        {
            this.EndAction(false);
            
            if (this.GoapSet != null)
                this.GoapSet.Unregister(this);
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
            where TGoal : IGoalBase
        {
            this.SetGoal(this.GoapSet.ResolveGoal<TGoal>(), endAction);
        }

        public void SetGoal(IGoalBase goal, bool endAction)
        {
            if (goal == this.CurrentGoal)
                return;
            
            this.CurrentGoal = goal;
            
            if (this.CurrentAction == null)
                this.GoapSet.Agents.Enqueue(this);
            
            this.Events.GoalStart(goal);
            
            if (endAction)
                this.EndAction();
        }

        public void SetAction(IActionBase action, List<IActionBase> path, ITarget target)
        {
            if (this.CurrentAction != null)
            {
                this.EndAction(false);
            }

            this.CurrentAction = action;

            var data = action.GetData();
            this.Injector.Inject(data);
            this.CurrentActionData = data;
            this.CurrentActionData.Target = target;
            this.CurrentActionPath = path;
            this.CurrentAction.Start(this, this.CurrentActionData);
            this.Events.ActionStart(action);
        }
        
        public void EndAction(bool enqueue = true)
        {
            var action = this.CurrentAction;
            
            this.CurrentAction?.End(this, this.CurrentActionData);
            this.CurrentAction = null;
            this.CurrentActionData = null;
            this.currentTarget = null;
            this.MoveState = AgentMoveState.Idle;
            
            this.Events.ActionStop(action);
            
            if (enqueue)
                this.GoapSet.Agents.Enqueue(this);
        }

        public void SetDistanceMultiplierSpeed(float speed)
        {
            this.DistanceMultiplier = 1f / speed;
        }
    }
}