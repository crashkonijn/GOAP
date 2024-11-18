using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class TurnBehaviour : MonoBehaviour
    {
        private List<GoapActionProvider> agents = new();
        private Queue<GoapActionProvider> queue = new();
        private GoapActionProvider activeAgent;

        [SerializeField]
        private TurnState state = TurnState.Idle;

        public void Register(GoapActionProvider agent)
        {
            this.agents.Add(agent);
        }

        private void Update()
        {
            if (this.state != TurnState.Idle)
                return;

            if (this.activeAgent != null)
                return;

            if (this.queue.Count == 0)
                this.queue = new Queue<GoapActionProvider>(this.agents);

            this.activeAgent = this.queue.Dequeue();
            this.StartTurn(this.activeAgent);
        }

        private void StartTurn(GoapActionProvider agent)
        {
            this.StartMoving(agent);
        }

        private void StartMoving(GoapActionProvider agent)
        {
            this.state = TurnState.Moving;

            var move = agent.GetComponent<GridMoveBehaviour>();
            move.Move((complete) =>
            {
                if (complete)
                {
                    this.StartActing(agent);
                    return;
                }

                this.StopTurn(agent);
            });
        }

        private void StartActing(GoapActionProvider agent)
        {
            this.state = TurnState.Acting;
            this.StopTurn(agent);
        }

        private void StopTurn(GoapActionProvider agent)
        {
            this.activeAgent = null;

            this.StartCoroutine(this.Wait());
        }

        private IEnumerator Wait()
        {
            this.state = TurnState.Waiting;
            yield return new WaitForSeconds(1f);
            this.state = TurnState.Idle;
        }
    }

    public enum TurnState
    {
        Idle,
        Moving,
        Acting,
        Waiting,
    }
}
