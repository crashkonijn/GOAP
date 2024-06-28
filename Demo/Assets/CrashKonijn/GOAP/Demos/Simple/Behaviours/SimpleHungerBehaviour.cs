using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Simple.Goap.Actions;
using CrashKonijn.Goap.Editor.Attributes;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class SimpleHungerBehaviour : MonoBehaviour
    {
        private AgentBehaviour agent;
        public float hunger = 50;

        [Button(nameof(EnablePickup))]
        public int enableButton;
        
        [Button(nameof(DisablePickup))]
        public int disableButton;

        private void Awake()
        {
            this.hunger = Random.Range(0, 100f);
            this.agent = this.GetComponent<AgentBehaviour>();
        }

        private void FixedUpdate()
        {
            this.hunger += Time.fixedDeltaTime * 2f;
        }

        public void EnablePickup()
        {
            this.agent.EnableAction<PickupAppleAction>();
        }

        public void DisablePickup()
        {
            this.agent.DisableAction<PickupAppleAction>();
        }
    }
}