using CrashKonijn.Goap.Core.Interfaces;
using UnityEngine;

namespace CrashKonijn.Goap.Behaviours
{
    public abstract class AgentTypeFactoryBase : MonoBehaviour
    {
        public abstract IAgentTypeConfig Create();
    }
}