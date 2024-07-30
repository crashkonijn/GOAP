using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Runtime;
using TMPro;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class ComplexTextBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private GoapActionProvider actionProvider;
        private ComplexHungerBehaviour simpleComplexHunger;
        private ComplexAgentBrain brain;
        private AgentBehaviour agent;

        private void Awake()
        {
            this.text = this.GetComponentInChildren<TextMeshProUGUI>();
            this.actionProvider = this.GetComponent<GoapActionProvider>();
            this.agent = this.GetComponent<AgentBehaviour>();
            this.simpleComplexHunger = this.GetComponent<ComplexHungerBehaviour>();
            this.brain = this.GetComponent<ComplexAgentBrain>();
        }

        private void Update()
        {
            this.text.text = this.GetText();
        }

        private string GetText()
        {
            if (this.actionProvider.CurrentPlan is null)
                return $"{this.GetTypeText()}\nIdle";
            
            if (this.actionProvider.Receiver.ActionState.Action is null)
                return $"{this.GetTypeText()}\nIdle";

            return $"{this.GetTypeText()}\n{this.actionProvider.CurrentPlan.GetType().GetGenericTypeName()}\n{this.actionProvider.Receiver.ActionState.Action.GetType().GetGenericTypeName()}\n{this.agent.State}\nhunger: {this.simpleComplexHunger.hunger:0.00}";
        }

        private string GetTypeText()
        {
            return this.brain.agentType.ToString();
        }
    }
}