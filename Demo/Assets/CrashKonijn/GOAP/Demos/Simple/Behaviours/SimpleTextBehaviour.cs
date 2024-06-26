using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using TMPro;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class SimpleTextBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private GoapAgentBehaviour agent;
        private SimpleHungerBehaviour simpleHunger;

        private void Awake()
        {
            this.text = this.GetComponentInChildren<TextMeshProUGUI>();
            this.agent = this.GetComponent<GoapAgentBehaviour>();
            this.simpleHunger = this.GetComponent<SimpleHungerBehaviour>();
        }

        private void Update()
        {
            this.text.text = this.GetText();
        }

        private string GetText()
        {
            if (this.agent.CurrentGoal is null)
                return "Idle";
            
            if (this.agent.Agent.ActionState.Action is null)
                return "Idle";

            return $"{this.agent.CurrentGoal.GetType().GetGenericTypeName()}\n{this.agent.Agent.ActionState.Action.GetType().GetGenericTypeName()}\n{this.agent.Agent.State}\nhunger: {this.simpleHunger.hunger:0.00}";
        }
    }
}