using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public class VectorDistanceObserver : IAgentDistanceObserver
    {
        public float GetDistance(IMonoAgent agent, ITarget target, IComponentReference reference)
        {
            if (agent.transform == null)
                return 0f;

            if (target == null)
                return 0f;

            if (!target.IsValid())
                return 0f;

            return Vector3.Distance(agent.transform.position, target.Position);
        }
    }
}
