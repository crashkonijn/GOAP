using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using UnityEngine;

namespace Demos.Complex.Sensors.Target
{
    public class ClosestItemSensor<T> : LocalTargetSensorBase
        where T : ItemBase
    {
        private readonly ItemCollection collection;
        private T[] items;

        public ClosestItemSensor()
        {
            this.collection = GameObject.FindObjectOfType<ItemCollection>();
        }
        
        public override void Update()
        {
            this.items = this.collection.Get<T>();
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