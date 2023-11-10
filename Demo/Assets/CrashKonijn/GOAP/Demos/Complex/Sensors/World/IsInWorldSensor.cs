using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Sensors.World
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