using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Behaviours
{
    public class ComplexHungerBehaviour : MonoBehaviour
    {
        public float hunger = 50;
        private IAgent agent;

        private void Awake()
        {
            this.agent = this.GetComponent<IAgent>();
            this.hunger = Random.Range(0, 100f);
            // this.hunger = 80f;
        }

        private void FixedUpdate()
        {
            if (this.agent.IsPaused)
                return;

            this.hunger += Time.fixedDeltaTime * 2f;
        }
    }
}
