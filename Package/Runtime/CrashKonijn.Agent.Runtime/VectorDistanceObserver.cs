using CrashKonijn.Agent.Core;
using UnityEngine;

namespace CrashKonijn.Agent.Runtime
{
    public class VectorDistanceObserver : IAgentDistanceObserver
    {
        public float GetDistance(IMonoAgent agent, ITarget target, IComponentReference reference)
        {
            if (target == null)
            {
                return 0f;
            }

            return Vector3.Distance(agent.transform.position, target.Position);
        }
    }
}
