using CrashKonijn.Agent.Core;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.World
{
    public class IsInWorldSensor<T> : LocalWorldSensorBase where T : IHoldable
    {
        private ItemCollection collection;

        public override void Created()
        {
            this.collection = Object.FindObjectOfType<ItemCollection>();
        }

        public override void Update()
        {
        }

        public override SenseValue Sense(IActionReceiver agent, IComponentReference references)
        {
            return this.collection.GetFiltered<T>(false, true, agent.Transform.gameObject).Length;
        }
    }
}