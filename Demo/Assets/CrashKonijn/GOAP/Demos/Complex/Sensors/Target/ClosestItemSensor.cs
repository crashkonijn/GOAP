using System.Linq;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.Target
{
    public class ClosestItemSensor<T> : LocalTargetSensorBase
        where T : IHoldable
    {
        private ItemCollection collection;

        public override void Created()
        {
            this.collection = Object.FindObjectOfType<ItemCollection>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closest = this.collection.GetFiltered<T>(false, true, agent.gameObject).Cast<ItemBase>().Closest(agent.transform.position);
            
            if (closest == null)
                return null;
            
            return new TransformTarget(closest.transform);
        }
    }
}