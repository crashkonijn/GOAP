using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Complex.Behaviours;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Sensors.World
{
    public class IsInWorldSensor<T> : GlobalWorldSensorBase where T : IHoldable
    {
        private ItemCollection collection;

        public override void Created()
        {
            this.collection = Object.FindObjectOfType<ItemCollection>();
        }

        public override SenseValue Sense()
        {
            return this.collection.GetFiltered<T>(false, true, false).Length;
        }
    }
}