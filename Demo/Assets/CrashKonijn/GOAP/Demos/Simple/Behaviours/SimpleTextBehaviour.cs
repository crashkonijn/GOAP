using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using TMPro;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class SimpleTextBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private AgentBehaviour agent;
        private SimpleHungerBehaviour simpleHunger;

        private void Awake()
        {
            this.text = this.GetComponentInChildren<TextMeshProUGUI>();
            this.agent = this.GetComponent<AgentBehaviour>();
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
            
            if (this.agent.ActionState.Action is null)
                return "Idle";

            return $"{this.agent.CurrentGoal.GetType().GetGenericTypeName()}\n{this.agent.ActionState.Action.GetType().GetGenericTypeName()}\n{this.agent.State}\nhunger: {this.simpleHunger.hunger:0.00}";
        }
    }
}