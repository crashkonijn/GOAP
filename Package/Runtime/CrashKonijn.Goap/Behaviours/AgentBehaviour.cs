using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public delegate void ActionDelegate(IActionBase action);
    public delegate void GoalDelegate(IGoalBase goal);
    
    public interface IMonoAgent : IAgent, IMonoBehaviour
    {
        
    }
    
    public interface IAgent
    {
        event ActionDelegate OnActionStart;
        event ActionDelegate OnActionStop;
        event GoalDelegate OnGoalStart;
        event GoalDelegate OnNoActionFound;
        
        AgentState State { get; }
        IGoapSet GoapSet { get; }
        IGoalBase CurrentGoal { get; }
        IActionBase CurrentAction { get; }
        IActionData CurrentActionData { get; }
        IWorldData WorldData { get; }
        List<IActionBase> CurrentActionPath { get; }

        void Run();
        
        void SetGoal<TGoal>(bool endAction) where TGoal : IGoalBase;

        void SetGoal(IGoalBase goal, bool endAction);
        void SetAction(IActionBase action, List<IActionBase> path, ITarget target);
        internal void FailedResolve();
    }

    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        public event ActionDelegate OnActionStart;
        public event ActionDelegate OnActionStop;
        public event GoalDelegate OnGoalStart;
        public event GoalDelegate OnNoActionFound;
        
        private IAgentMover mover;
        
        public GoapSetBehaviour goapSet;

        public IAgentMover Mover => this.mover;
        
        public AgentState State { get; private set; } = AgentState.NoAction;
        public IGoapSet GoapSet { get; set; }
        public IGoalBase CurrentGoal { get; private set; }
        public IActionBase CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public IWorldData WorldData { get; } = new LocalWorldData();
        public List<IActionBase> CurrentActionPath { get; private set; } = new List<IActionBase>();
        
        private DataReferenceInjector injector;

        private void Awake()
        {
            this.injector = new DataReferenceInjector(this);
            this.mover = this.GetComponent<IAgentMover>();
            
            if (this.goapSet != null)
                this.GoapSet = this.goapSet.Set;
        }

        private void OnEnable()
        {
            this.GoapSet.Register(this);
        }

        private void OnDisable()
        {
            this.GoapSet.Unregister(this);
        }

        public void Run()
        {
            if (this.CurrentAction == null)
            {
                this.State = AgentState.NoAction;
                return;
            }
            
            if (this.GetDistance() <= this.CurrentAction.Config.InRange)
            {
                this.PerformAction();
                return;
            }

            this.Move();
        }

        private void Move()
        {
            this.State = AgentState.MovingToTarget;
            
            this.mover.Move(this.CurrentActionData.Target);
        }

        private void PerformAction()
        {
            this.State = AgentState.PerformingAction;

            var result = this.CurrentAction.Perform(this, this.CurrentActionData, new ActionContext
            {
                DeltaTime = Time.deltaTime,
            });

            if (result == ActionRunState.Continue)
                return;
            
            this.EndAction();
        }

        private float GetDistance()
        {
            if (this.CurrentActionData?.Target == null)
            {
                return 0f;
            }

            return Vector3.Distance(this.transform.position, this.CurrentActionData.Target.Position);
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
            
            this.OnGoalStart?.Invoke(goal);
            
            if (endAction)
                this.EndAction();
        }

        public void SetAction(IActionBase action, List<IActionBase> path, ITarget target)
        {
            if (this.CurrentAction != null)
            {
                this.EndAction();
            }

            this.CurrentAction = action;

            var data = action.GetData();
            this.injector.Inject(data);
            this.CurrentActionData = data;
            this.CurrentActionData.Target = target;
            this.CurrentAction.OnStart(this, this.CurrentActionData);
            this.CurrentActionPath = path;
            this.OnActionStart?.Invoke(action);
        }

        void IAgent.FailedResolve()
        {
            this.OnNoActionFound?.Invoke(this.CurrentGoal);
        }

        private void EndAction()
        {
            var action = this.CurrentAction;
            
            this.CurrentAction?.OnEnd(this, this.CurrentActionData);
            this.CurrentAction = null;
            this.CurrentActionData = null;
            
            this.GoapSet.Agents.Enqueue(this);
            this.OnActionStop?.Invoke(action);
        }
    }
}