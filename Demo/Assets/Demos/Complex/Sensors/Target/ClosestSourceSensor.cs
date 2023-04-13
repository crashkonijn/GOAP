using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Sensors.Target
{
    public class ClosestSourceSensor<T> : LocalTargetSensorBase
        where T : IGatherable
    {
        private ItemSourceBase<T>[] collection;
        
        public override void Created()
        {
            this.collection = GameObject.FindObjectsOfType<ItemSourceBase<T>>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closest = this.collection.Closest(agent.transform.position);
            
            if (closest == null)
                return null;
            
            return new TransformTarget(closest.transform);
        }
    }
}