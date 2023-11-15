using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Sensors;
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
            this.items = Object.FindObjectsOfType<T>();
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closest = this.items.Closest(agent.transform.position);
            
            if (closest == null)
                return null;
            
            return new TransformTarget(closest.transform);
        }
    }
}