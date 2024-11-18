using CrashKonijn.Agent.Runtime;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class TurnAnimationBehaviour : MonoBehaviour
    {
        private Animator animator;
        private AgentBehaviour agent;
        private GridMoveBehaviour move;
        private static readonly int Walking = Animator.StringToHash("Walking");

        private bool isWalking;
        private bool isMovingLeft;

        private void Awake()
        {
            this.animator = this.GetComponentInChildren<Animator>();
            this.agent = this.GetComponent<AgentBehaviour>();
            this.move = this.GetComponent<GridMoveBehaviour>();

            // Random y offset to prevent clipping
            this.animator.transform.localPosition = new Vector3(0, Random.Range(-0.1f, 0.1f), 0);
        }

        private void Update()
        {
            this.UpdateAnimation();
            this.UpdateScale();
        }

        private void UpdateAnimation()
        {
            var isWalking = this.move.IsMoving;

            if (this.isWalking == isWalking)
                return;

            this.isWalking = isWalking;

            this.animator.SetBool(Walking, isWalking);
        }

        private void UpdateScale()
        {
            if (!this.isWalking)
                return;

            var isMovingLeft = this.IsMovingLeft();

            if (this.isMovingLeft == isMovingLeft)
                return;

            this.isMovingLeft = isMovingLeft;

            this.animator.transform.localScale = new Vector3(isMovingLeft ? -1 : 1, 1, 1);
        }

        private bool IsMovingLeft()
        {
            var target = this.move.MovePosition;

            return this.transform.position.x > target.x;
        }
    }
}
