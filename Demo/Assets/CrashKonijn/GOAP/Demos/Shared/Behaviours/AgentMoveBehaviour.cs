using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using UnityEngine;

namespace Demos.Shared.Behaviours
{
    public class AgentMoveBehaviour : MonoBehaviour
    {
        private AgentBehaviour agent;
        private ITarget currentTarget;
        public bool IsMoving { get; private set; }

        private void Awake()
        {
            this.agent = this.GetComponent<AgentBehaviour>();
        }

        private void OnEnable()
        {
            this.agent.Events.OnTargetInRange += this.OnTargetInRange;
            this.agent.Events.OnTargetChanged += this.OnTargetChanged;
            this.agent.Events.OnTargetNotInRange += this.TargetNotInRange;
            this.agent.Events.OnTargetLost += this.TargetLost;
        }

        private void OnDisable()
        {
            this.agent.Events.OnTargetInRange -= this.OnTargetInRange;
            this.agent.Events.OnTargetChanged -= this.OnTargetChanged;
            this.agent.Events.OnTargetNotInRange -= this.TargetNotInRange;
            this.agent.Events.OnTargetLost -= this.TargetLost;
        }

        private void TargetLost()
        {
            this.currentTarget = null;
            this.IsMoving = false;
        }

        private void OnTargetInRange(ITarget target)
        {
            this.IsMoving = false;
        }

        private void OnTargetChanged(ITarget target, bool inRange)
        {
            this.currentTarget = target;
            this.IsMoving = !inRange;
        }

        private void TargetNotInRange(ITarget target)
        {
            this.IsMoving = true;
        }

        public void Update()
        {
            if (!this.IsMoving)
                return;
            
            if (this.currentTarget == null)
                return;
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.currentTarget.Position.x, this.transform.position.y, this.currentTarget.Position.z), Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (this.currentTarget == null)
                return;
            
            Gizmos.DrawLine(this.transform.position, this.currentTarget.Position);
        }
    }
}