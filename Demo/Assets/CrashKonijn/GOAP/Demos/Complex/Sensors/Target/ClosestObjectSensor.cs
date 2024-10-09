using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.Target
{
    public class ClosestObjectSensor<T> : LocalTargetSensorBase
        where T : MonoBehaviour
    {
        private T[] items;

        public override void Created()
        {
        }

        public override void Update()
        {
            this.items = Compatibility.FindObjectsOfType<T>();
        }

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var closest = this.items.Closest(agent.Transform.position);
            
            if (closest == null)
                return null;
            
            // Re-use the target if it already exists
            if (target is TransformTarget transformTarget)
                return transformTarget.SetTransform(closest.transform);
            
            return new TransformTarget(closest.transform);
        }
    }
}