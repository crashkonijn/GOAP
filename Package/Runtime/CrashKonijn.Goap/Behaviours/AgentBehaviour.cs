using System.Collections.Generic;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrashKonijn.Goap.Behaviours
{
    public delegate void ActionDelegate(IActionBase action);
    public delegate void GoalDelegate(IGoalBase goal);
    
    public interface IMonoAgent : IAgent, IMonoBehaviour
    {
        
    }
    
    public interface IAgent
    {
        AgentState State { get; }
        IGoapSet GoapSet { get; }
        IGoalBase CurrentGoal { get; }
        IActionBase CurrentAction { get; }
        IActionData CurrentActionData { get; }
        IWorldData WorldData { get; }
        List<IActionBase> CurrentActionPath { get; }
        IAgentEvents Events { get; }
        IDataReferenceInjector Injector { get; }

        void Run();
        
        void SetGoal<TGoal>(bool endAction) where TGoal : IGoalBase;

        void SetGoal(IGoalBase goal, bool endAction);
        void SetAction(IActionBase action, List<IActionBase> path, ITarget target);
    }

    public interface IAgentEvents
    {
        event ActionDelegate OnActionStart;
        void ActionStart(IActionBase action);
        
        event ActionDelegate OnActionStop;
        void ActionStop(IActionBase action);
        
        event GoalDelegate OnGoalStart;
        void GoalStart(IGoalBase goal);
        
        event GoalDelegate OnNoActionFound;
        void NoActionFound(IGoalBase goal);
    }
    
    public class AgentEvents : IAgentEvents
    {
        public event ActionDelegate OnActionStart;
        public void ActionStart(IActionBase action)
        {
            this.OnActionStart?.Invoke(action);
        }
        
        public event ActionDelegate OnActionStop;
        public void ActionStop(IActionBase action)
        {
            this.OnActionStop?.Invoke(action);
        }
        
        public event GoalDelegate OnGoalStart;
        public void GoalStart(IGoalBase goal)
        {
            this.OnGoalStart?.Invoke(goal);
        }
        
        public event GoalDelegate OnNoActionFound;
        public void NoActionFound(IGoalBase goal)
        {
            this.OnNoActionFound?.Invoke(goal);
        }
    }

    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        private IAgentMover mover;
        
        public GoapSetBehaviour goapSetBehaviour;

        public IAgentMover Mover => this.mover;
        
        public AgentState State { get; private set; } = AgentState.NoAction;

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
        public IWorldData WorldData { get; } = new LocalWorldData();
        public List<IActionBase> CurrentActionPath { get; private set; } = new List<IActionBase>();
        public IAgentEvents Events { get; } = new AgentEvents();
        public IDataReferenceInjector Injector { get; private set; }

        private void Awake()
        {
            this.Injector = new DataReferenceInjector(this);
            this.mover = this.GetComponent<IAgentMover>();
            
            if (this.goapSetBehaviour != null)
                this.GoapSet = this.goapSetBehaviour.Set;
        }

        private void OnEnable()
        {
            if (this.GoapSet != null)
                this.GoapSet.Register(this);
        }

        private void OnDisable()
        {
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
            
            this.Events.GoalStart(goal);
            
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
            this.Injector.Inject(data);
            this.CurrentActionData = data;
            this.CurrentActionData.Target = target;
            this.CurrentAction.Start(this, this.CurrentActionData);
            this.CurrentActionPath = path;
            this.Events.ActionStart(action);
        }
        
        private void EndAction()
        {
            var action = this.CurrentAction;
            
            this.CurrentAction?.End(this, this.CurrentActionData);
            this.CurrentAction = null;
            this.CurrentActionData = null;
            
            this.GoapSet.Agents.Enqueue(this);
            this.Events.ActionStop(action);
        }
    }
}