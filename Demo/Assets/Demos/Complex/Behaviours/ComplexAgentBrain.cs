using System;
using CrashKonijn.Goap.Behaviours;
using Demos.Shared.Goals;
using UnityEngine;

namespace Demos.Complex.Behaviours
{
    public class ComplexAgentBrain : MonoBehaviour
    {
        private AgentBehaviour agent;

        private void Awake()
        {
            this.agent = this.GetComponent<AgentBehaviour>();
        }

        private void Start()
        {
            this.agent.SetGoal<WanderGoal>(false);
        }

        private void Update()
        {
            
        }
    }
}