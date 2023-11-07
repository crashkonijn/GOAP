using CrashKonijn.Goap.Configs.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class AgentTypeFactoryBase : MonoBehaviour
    {
        public abstract IAgentTypeConfig Create();
    }
    
    public abstract class BehaviourSetFactoryBase : MonoBehaviour
    {
        public abstract IAgentTypeConfig Create();
    }
}