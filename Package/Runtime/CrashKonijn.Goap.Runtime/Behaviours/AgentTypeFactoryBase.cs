using CrashKonijn.Goap.Core;
using UnityEngine;

namespace CrashKonijn.Goap.Runtime
{
    public abstract class AgentTypeFactoryBase : MonoBehaviour
    {
        public abstract IAgentTypeConfig Create();
    }
}