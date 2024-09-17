using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;
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

        public override ITarget Sense(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var closest = this.collection.GetFiltered<T>(false, true, agent.Transform.gameObject).Cast<ItemBase>().Closest(agent.Transform.position);
            
            if (closest == null)
                return null;
            
            // Re-use the existing target if the target exists
            if (target is TransformTarget targetTransform)
                return targetTransform.SetTransform(closest.transform);
            
            return new TransformTarget(closest.transform);
        }
    }
}