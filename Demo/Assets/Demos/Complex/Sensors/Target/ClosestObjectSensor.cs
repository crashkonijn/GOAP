using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace Demos.Complex.Sensors.Target
{
    public class ClosestObjectSensor<T> : LocalTargetSensorBase
        where T : MonoBehaviour
    {
        private T[] items;

        public override void Update()
        {
            this.items = GameObject.FindObjectsOfType<T>();
        }

        public override ITarget Sense(IMonoAgent agent)
        {
            var closest = this.items.Closest(agent.transform.position);
            
            if (closest == null)
                return null;
            
            return new TransformTarget(closest.transform);
        }
    }
}