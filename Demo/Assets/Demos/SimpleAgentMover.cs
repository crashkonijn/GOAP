using System;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace Demos
{
    public class SimpleAgentMover : MonoBehaviour, IAgentMover
    {
        private ITarget target;
        
        public void Move(ITarget target)
        {
            this.target = target;
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.Position, Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (this.target == null)
                return;
            
            Gizmos.DrawLine(this.transform.position, this.target.Position);
        }
    }
}