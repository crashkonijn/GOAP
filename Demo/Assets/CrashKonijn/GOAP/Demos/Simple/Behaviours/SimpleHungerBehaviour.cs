using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Demos.Simple.Goap.Actions;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class SimpleHungerBehaviour : MonoBehaviour
    {
        private GoapActionProvider actionProvider;
        public float hunger = 50;

        [Button(nameof(EnablePickup))]
        public int enableButton;

        [Button(nameof(DisablePickup))]
        public int disableButton;

        private void Awake()
        {
            this.hunger = Random.Range(0, 100f);
            this.actionProvider = this.GetComponent<GoapActionProvider>();
        }

        private void FixedUpdate()
        {
            this.hunger += Time.fixedDeltaTime * 2f;
        }

        public void EnablePickup()
        {
            foreach (var pickupAppleAction in this.actionProvider.GetActions<PickupAppleAction>())
            {
                this.actionProvider.Enable(pickupAppleAction);
            }
        }

        public void DisablePickup()
        {
            foreach (var pickupAppleAction in this.actionProvider.GetActions<PickupAppleAction>())
            {
                this.actionProvider.Disable(pickupAppleAction, ActionDisabler.ForTime(1f));
            }
        }
    }
}
