using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class HungerBehaviour : MonoBehaviour
    {
        public float hunger = 50;

        private void Awake()
        {
            this.hunger = Random.Range(0, 100f);
            // this.hunger = 80f;
        }

        private void FixedUpdate()
        {
            this.hunger += Time.fixedDeltaTime * 2f;
        }
    }
}