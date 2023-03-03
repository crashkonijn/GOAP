using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Enums;
using UnityEngine;

namespace Demos.Behaviours
{
    public class AnimationBehaviour : MonoBehaviour
    {
        private Animator animator;
        private AgentBehaviour agent;
        private static readonly int Walking = Animator.StringToHash("Walking");

        private void Awake()
        {
            this.animator = this.GetComponentInChildren<Animator>();
            this.agent = this.GetComponent<AgentBehaviour>();
            
            // Random y offset to prevent clipping
            this.animator.transform.localPosition = new Vector3(0, UnityEngine.Random.Range(-0.1f, 0.1f), 0);
        }

        private void Update()
        {
            var isWalking = this.agent.State == AgentState.MovingToTarget;
            
            this.animator.SetBool(Walking, isWalking);
            
            if (!isWalking)
                return;
            
            this.animator.transform.localScale = new Vector3(this.IsMovingLeft() ? -1 : 1, 1, 1);
        }

        private bool IsMovingLeft()
        {
            var target = this.agent.CurrentActionData.Target.Position;
            
            return this.transform.position.x > target.x;
        }
    }
}