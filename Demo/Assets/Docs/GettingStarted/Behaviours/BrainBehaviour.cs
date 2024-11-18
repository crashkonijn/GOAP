using System;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Docs.GettingStarted.Behaviours
{
    public class BrainBehaviour : MonoBehaviour
    {
        private AgentBehaviour agent;
        private GoapActionProvider provider;
        private GoapBehaviour goap;
        private DataBehaviour data;
        
        private void Awake()
        {
            this.goap = FindObjectOfType<GoapBehaviour>();
            this.agent = this.GetComponent<AgentBehaviour>();
            this.provider = this.GetComponent<GoapActionProvider>();
            this.data = this.GetComponent<DataBehaviour>();
            
            // This only applies sto the code demo
            if (this.provider.AgentTypeBehaviour == null)
                this.provider.AgentType = this.goap.GetAgentType("DemoAgent");
        }

        private void Start()
        {
            this.provider.RequestGoal<IdleGoal, PickupPearGoal>();
        }

        private void OnEnable()
        {
            this.agent.Events.OnActionEnd += this.OnActionEnd;
        }

        private void OnDisable()
        {
            this.agent.Events.OnActionEnd -= this.OnActionEnd;
        }

        private void OnActionEnd(IAction action)
        {
            if (this.data.hunger > 50)
            {
                this.provider.RequestGoal<EatGoal>();
                return;
            }
            
            this.provider.RequestGoal<IdleGoal, PickupPearGoal>();
        }
    }
}