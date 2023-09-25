using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using Demos.Shared.Goals;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased
{
    public class TurnAgentBrain : MonoBehaviour
    {
        private IMonoAgent agent;

        private void Awake()
        {
            this.agent = this.GetComponent<AgentBehaviour>();
        }

        private void Start()
        {
            this.agent.SetGoal<WanderGoal>(true);
        }
    }
}