using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Validators;
using Demos.Shared.Behaviours;
using TMPro;
using UnityEngine;

namespace Demos.Simple.Behaviours
{
    public class TextBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private AgentBehaviour agent;
        private HungerBehaviour hunger;

        private void Awake()
        {
            this.text = this.GetComponentInChildren<TextMeshProUGUI>();
            this.agent = this.GetComponent<AgentBehaviour>();
            this.hunger = this.GetComponent<HungerBehaviour>();
        }

        private void Update()
        {
            this.text.text = this.GetText();
        }

        private string GetText()
        {
            if (this.agent.CurrentAction is null)
                return "Idle";

            return $"{this.agent.CurrentGoal.GetType().GetGenericTypeName()}\n{this.agent.CurrentAction.GetType().GetGenericTypeName()}\n{this.agent.State}\nhunger: {this.hunger.hunger:0.00}";
        }
    }
}