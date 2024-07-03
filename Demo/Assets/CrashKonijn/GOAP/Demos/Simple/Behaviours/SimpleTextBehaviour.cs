using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using TMPro;
using UnityEngine;
using Extensions = CrashKonijn.Goap.Runtime.Extensions;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class SimpleTextBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private GoapActionProvider actionProvider;
        private SimpleHungerBehaviour simpleHunger;
        private AgentBehaviour agent;

        private void Awake()
        {
            this.text = this.GetComponentInChildren<TextMeshProUGUI>();
            this.actionProvider = this.GetComponent<GoapActionProvider>();
            this.agent = this.GetComponent<AgentBehaviour>();
            this.simpleHunger = this.GetComponent<SimpleHungerBehaviour>();
        }

        private void Update()
        {
            this.text.text = this.GetText();
        }

        private string GetText()
        {
            if (this.actionProvider.CurrentPlan is null)
                return "Idle";
            
            if (this.actionProvider.Receiver.ActionState.Action is null)
                return "Idle";

            return $"{this.actionProvider.CurrentPlan.Goal.GetType().GetGenericTypeName()}\n{this.actionProvider.Receiver.ActionState.Action.GetType().GetGenericTypeName()}\n{this.agent.State}\nhunger: {this.simpleHunger.hunger:0.00}";
        }
    }
}