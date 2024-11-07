using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;
using CrashKonijn.Goap.Runtime;
using UnityEngine;
using TransformTarget = CrashKonijn.Goap.Runtime.TransformTarget;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.Multi
{
    public class ItemSensor<T> : MultiSensorBase
        where T : class, IHoldable
    {
        private ItemCollection collection;

        public ItemSensor()
        {
            this.AddLocalTargetSensor<ClosestTarget<T>>(this.SenseClosestTarget);
            this.AddLocalWorldSensor<IsInWorld<T>>(this.SenseIsInWorld);
        }

        public override void Created()
        {
            this.collection = Object.FindObjectOfType<ItemCollection>();
        }

        public override void Update() { }

        private ITarget SenseClosestTarget(IActionReceiver agent, IComponentReference references, ITarget target)
        {
            var closest = this.collection.GetFiltered<T>(false, true, agent.Transform.gameObject).Cast<ItemBase>().Closest(agent.Transform.position);

            if (closest == null)
                return null;

            // Re-use the existing target if the target exists
            if (target is TransformTarget targetTransform)
                return targetTransform.SetTransform(closest.transform);

            return new TransformTarget(closest.transform);
        }

        private SenseValue SenseIsInWorld(IActionReceiver agent, IComponentReference references)
        {
            return this.collection.GetFiltered<T>(false, true, agent.Transform.gameObject).Length;
        }
    }
}
