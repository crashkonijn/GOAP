using System.Collections.Generic;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public class Agent : MonoBehaviour
    {
        public GoapSet goapSet;

        public IGoalBase CurrentGoal { get; private set; }
        public IActionBase CurrentAction  { get;  private set;}
        public IActionData CurrentActionData { get; private set; }
        public IWorldData WorldData { get; private set; }
        public List<IActionBase> CurrentActionPath { get; private set; }

        private IAgentMover mover;

        private void Awake()
        {
            this.mover = this.GetComponent<IAgentMover>();
        }

        private void OnEnable()
        {
            this.goapSet.Register(this);
        }

        private void OnDisable()
        {
            this.goapSet.Unregister(this);
        }

        public void Update()
        {
            if (this.CurrentAction == null)
                return;
            
            if (this.GetDistance() <= this.CurrentAction.Config.inRange)
                return;
            
            this.mover.Move(this.CurrentActionData.Target);
        }

        public void FixedUpdate()
        {
            if (this.CurrentAction == null)
                return;
            
            if (this.GetDistance() > this.CurrentAction.Config.inRange)
                return;

            var result = this.CurrentAction.Perform(this, this.CurrentActionData);

            if (result == ActionRunState.Continue)
                return;
            
            this.EndAction();
        }

        private float GetDistance()
        {
            if (this.CurrentActionData?.Target == null)
            {
                UnityEngine.Debug.Log(this.CurrentAction);
                return 0f;
            }

            return Vector3.Distance(this.transform.position, this.CurrentActionData.Target.Position);
        }

        public void SetGoal<TGoal>(bool endAction)
            where TGoal : IGoalBase
        {
            this.SetGoal(this.goapSet.ResolveGoal<TGoal>(), endAction);
        }

        public void SetGoal(IGoalBase goal, bool endAction)
        {
            if (goal == this.CurrentGoal)
                return;
            
            this.CurrentGoal = goal;
            
            if (endAction)
                this.EndAction();
        }

        public void SetWorldData(IWorldData worldData)
        {
            this.WorldData = worldData;
        }

        public void SetAction(IActionBase action, List<IActionBase> path, ITarget target)
        {
            if (this.CurrentAction != null)
            {
                this.EndAction();
            }

            UnityEngine.Debug.Log($"Starting action: {action}");
            this.CurrentAction = action;
            this.CurrentActionData = action.GetData();
            this.CurrentActionData.Target = target;
            this.CurrentAction.OnStart(this, this.CurrentActionData);
            this.CurrentActionPath = path;
        }


        private void EndAction()
        {
            UnityEngine.Debug.Log($"End action: {this.CurrentAction}");
            this.CurrentAction?.OnEnd(this, this.CurrentActionData);
            this.CurrentAction = null;
            this.CurrentActionData = null;
        }
    }
}