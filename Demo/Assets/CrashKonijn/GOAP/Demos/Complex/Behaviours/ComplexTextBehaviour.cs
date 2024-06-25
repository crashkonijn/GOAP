using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using TMPro;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class ComplexTextBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private AgentBehaviour agent;
        private ComplexHungerBehaviour simpleComplexHunger;
        private ComplexAgentBrain brain;

        private void Awake()
        {
            this.text = this.GetComponentInChildren<TextMeshProUGUI>();
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
            if (this.agent.CurrentGoal is null)
                return $"{this.GetTypeText()}\nIdle";
            
            if (this.agent.ActionState.Action is null)
                return $"{this.GetTypeText()}\nIdle";

            return $"{this.GetTypeText()}\n{this.agent.CurrentGoal.GetType().GetGenericTypeName()}\n{this.agent.ActionState.Action.GetType().GetGenericTypeName()}\n{this.agent.State}\nhunger: {this.simpleComplexHunger.hunger:0.00}";
        }

        private string GetTypeText()
        {
            return this.brain.agentType.ToString();
        }
    }
}