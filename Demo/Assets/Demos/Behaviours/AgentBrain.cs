using System;
using CrashKonijn.Goap.Behaviours;
using Demos.Goals;
using UnityEngine;

namespace Demos.Behaviours
{
    public class AgentBrain : MonoBehaviour
    {
        private AgentBehaviour agent;
        private HungerBehaviour hunger;

        private void Awake()
        {
            this.agent = this.GetComponent<AgentBehaviour>();
            this.hunger = this.GetComponent<HungerBehaviour>();
        }

        private void Start()
        {
            this.agent.SetGoal<WanderGoal>(false);
        }

        private void FixedUpdate()
        {
            if (this.hunger.hunger > 80)
                this.agent.SetGoal<FixHungerGoal>(false);
            
            if (this.hunger.hunger < 20)
                this.agent.SetGoal<WanderGoal>(true);
        }
    }
}