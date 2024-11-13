using System;
using System.Collections.Generic;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.GOAP.Demos.TurnBased.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class GridMoveBehaviour : MonoBehaviour
    {
        [field: SerializeField]
        public bool IsMoving { get; private set; }

        [field: SerializeField]
        public Vector3 MovePosition { get; private set; }

        private IMonoAgent agent;
        private ITarget target;

        [SerializeField]
        private int moveCount;

        [SerializeField]
        private int maxMoveCount = 3;

        private Action<bool> onComplete;
        private IGrid grid;
        private Queue<ITile> path = new();

        private void Awake()
        {
            this.agent = this.GetComponent<AgentBehaviour>();
            this.grid = FindObjectOfType<GridBehaviour>();
        }

        private void Update()
        {
            if (!this.IsMoving)
                return;

            var targetPos = this.MovePosition + new Vector3(0f, this.transform.position.y, 0f);

            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, Time.deltaTime);

            if (Vector3.Distance(this.transform.position, targetPos) >= 0.05f)
                return;

            this.transform.position = targetPos;

            this.moveCount++;

            if (this.moveCount >= this.maxMoveCount)
            {
                this.IsMoving = false;
                this.onComplete(false);
                return;
            }

            this.MovePosition = this.GetMovePosition();

            targetPos = this.MovePosition + new Vector3(0f, this.transform.position.y, 0f);

            if (this.transform.position == targetPos)
            {
                this.IsMoving = false;
                this.onComplete(true);
            }
        }

        private void OnEnable()
        {
            this.agent.Events.OnTargetChanged += this.OnTargetChanged;
        }

        private void OnDisable()
        {
            this.agent.Events.OnTargetChanged -= this.OnTargetChanged;
        }

        private void OnTargetChanged(ITarget target, bool inrange)
        {
            this.target = target;
        }

        public void Move(Action<bool> onComplete)
        {
            if (this.target == null)
            {
                onComplete(false);
                return;
            }

            this.IsMoving = true;
            this.onComplete = onComplete;
            this.moveCount = 0;
            this.path = new Queue<ITile>(this.grid.GetPath(this.transform.position, this.target.Position));
            this.MovePosition = this.GetMovePosition();
        }

        private Vector3 GetMovePosition()
        {
            var currentTile = this.grid.GetTile(this.transform.position);

            if (this.path.Count == 0)
                return this.target.Position;

            var nextTile = this.path.Dequeue();

            if (nextTile == null)
            {
                return this.transform.position;
            }

            if (currentTile == nextTile)
                return this.GetMovePosition();

            return nextTile.Position;
        }

        private void OnDrawGizmos()
        {
            // if (this.path == null)
            //     return;
            //
            // foreach (var tile in this.path)
            // {
            //     Gizmos.color = Color.red;
            //     Gizmos.DrawCube(tile.Position, Vector3.one * 0.5f);
            // }
        }
    }
}
