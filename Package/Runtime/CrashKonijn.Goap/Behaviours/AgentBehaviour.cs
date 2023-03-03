using System.Collections.Generic;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
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

        void SetGoal<TGoal>(bool endAction) where TGoal : IGoalBase;

        void SetGoal(IGoalBase goal, bool endAction);
        void SetAction(IActionBase action, List<IActionBase> path, ITarget target);
    }

    public class AgentBehaviour : MonoBehaviour, IMonoAgent
    {
        private IAgentMover mover;
        
        public GoapSetBehaviour goapSet;

        public IAgentMover Mover => this.mover;

        public AgentState State { get; private set; } = AgentState.NoAction;
        public IGoapSet GoapSet { get; set; }
        public IGoalBase CurrentGoal { get; private set; }
        public IActionBase CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public IWorldData WorldData { get; } = new LocalWorldData();
        public List<IActionBase> CurrentActionPath { get; private set; }
        

        private void Awake()
        {
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

        public void Update()
        {
            if (this.CurrentAction == null)
                return;
            
            if (this.GetDistance() <= this.CurrentAction.Config.inRange)
                return;

            this.State = AgentState.MovingToTarget;
            
            this.mover.Move(this.CurrentActionData.Target);
        }

        public void FixedUpdate()
        {
            if (this.CurrentAction == null)
            {
                this.State = AgentState.NoAction;
                return;
            }
            
            if (this.GetDistance() > this.CurrentAction.Config.inRange)
                return;
            
            this.State = AgentState.PerformingAction;

            var result = this.CurrentAction.Perform(this, this.CurrentActionData);

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
            
            if (endAction)
                this.EndAction();
        }

        public void SetAction(IActionBase action, List<IActionBase> path, ITarget target)
        {
            if (this.CurrentAction != null)
            {
                this.EndAction();
            }

            // UnityEngine.Debug.Log($"Starting action: {action}");
            this.CurrentAction = action;
            this.CurrentActionData = action.GetData();
            this.CurrentActionData.Target = target;
            this.CurrentAction.OnStart(this, this.CurrentActionData);
            this.CurrentActionPath = path;
        }


        private void EndAction()
        {
            // UnityEngine.Debug.Log($"End action: {this.CurrentAction}");
            this.CurrentAction?.OnEnd(this, this.CurrentActionData);
            this.CurrentAction = null;
            this.CurrentActionData = null;
        }
    }
}