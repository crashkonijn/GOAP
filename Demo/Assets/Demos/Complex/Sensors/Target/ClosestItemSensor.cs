using System.Linq;
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
    public class ClosestItemSensor<T> : LocalTargetSensorBase
        where T : IHoldable
    {
        private ItemCollection collection;
        private T[] items;

        public override void Created()
        {
            this.collection = GameObject.FindObjectOfType<ItemCollection>();
        }

        public override void Update()
        {
            this.items = this.collection.GetFiltered<T>(false, true, false);
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closest = this.items.Cast<ItemBase>().Closest(agent.transform.position);
            
            if (closest == null)
                return null;
            
            return new TransformTarget(closest.transform);
        }
    }
}