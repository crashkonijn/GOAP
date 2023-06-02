using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace Demos.Complex.Sensors.Target
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
            this.items = GameObject.FindObjectsOfType<T>();
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