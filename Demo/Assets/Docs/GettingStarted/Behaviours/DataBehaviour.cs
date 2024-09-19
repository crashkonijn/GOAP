using System;
using UnityEngine;

namespace CrashKonijn.Docs.GettingStarted.Behaviours
{
    public class DataBehaviour : MonoBehaviour
    {
        public int pearCount = 0;
        public float hunger = 0f;
        
        private void Update()
        {
            // For simplicity, we will increase the hunger over time in this class.
            this.hunger += Time.deltaTime * 5f;
        }
    }
}
