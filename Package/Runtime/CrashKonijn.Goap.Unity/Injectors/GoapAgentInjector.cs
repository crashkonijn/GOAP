using CrashKonijn.Goap.Behaviours;
using UnityEngine;

namespace CrashKonijn.Goap.Unity.Injectors
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(AgentBehaviour))]
    public class GoapAgentInjector : MonoBehaviour
    {
        [SerializeField]
        private GoapSetBehaviour goapSet;
        
        private void Awake()
        {
            this.GetComponent<AgentBehaviour>().Construct(this.goapSet.Set);
        }
    }
}