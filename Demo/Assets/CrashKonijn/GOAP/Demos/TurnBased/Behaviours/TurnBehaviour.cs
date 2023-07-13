using System;
using System.Collections;
using System.Collections.Generic;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased.Behaviours
{
    public class TurnBehaviour : MonoBehaviour
    {
        private List<IMonoAgent> agents = new();
        private Queue<IMonoAgent> queue = new();
        private IMonoAgent activeAgent;
        
        [SerializeField]
        private TurnState state = TurnState.Idle;

        public void Register(IMonoAgent agent)
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
                this.queue = new Queue<IMonoAgent>(this.agents);
            
            this.activeAgent = this.queue.Dequeue();
            this.StartTurn(this.activeAgent);
        }

        private void StartTurn(IMonoAgent agent)
        {
            this.StartMoving(agent);
        }

        private void StartMoving(IMonoAgent agent)
        {
            this.state = TurnState.Moving;
            
            var move = agent.GetComponent<GridMoveBehaviour>();
            move.Move((complete) =>
            {
                Debug.Log(complete);
                if (complete)
                {
                    this.StartActing(agent);
                    return;
                }
                
                this.StopTurn(agent);
            });
        }

        private void StartActing(IMonoAgent agent)
        {
            this.state = TurnState.Acting;
            this.StopTurn(agent);
        }

        private void StopTurn(IMonoAgent agent)
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
        Waiting
    }
}