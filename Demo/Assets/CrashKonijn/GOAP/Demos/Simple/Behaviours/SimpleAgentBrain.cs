using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Demos.Simple.Goap.Goals;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Behaviours
{
    public class SimpleAgentBrain : MonoBehaviour
    {
        private GoapActionProvider agent;
        private SimpleHungerBehaviour hunger;

        private void Awake()
        {
            this.agent = this.GetComponent<GoapActionProvider>();
            this.hunger = this.GetComponent<SimpleHungerBehaviour>();
        }

        private void Start()
        {
            this.agent.SetGoal<WanderGoal>(false);
        }

        private void Update()
        {
            if (this.hunger.hunger > 80)
            {
                this.agent.SetGoal<FixHungerGoal>(false);
                return;
            }
            
            if (this.hunger.hunger < 20)
                this.agent.SetGoal<WanderGoal>(true);
        }
    }
}