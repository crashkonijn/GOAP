using CrashKonijn.Goap.Demos.TurnBased;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.GOAP.Demos.TurnBased
{
    public class TurnAgentBrain : MonoBehaviour
    {
        private GoapActionProvider agent;

        private void Awake()
        {
            this.agent = this.GetComponent<GoapActionProvider>();
        }

        private void Start()
        {
            this.agent.RequestGoal<WanderGoal>();
        }
    }
}
