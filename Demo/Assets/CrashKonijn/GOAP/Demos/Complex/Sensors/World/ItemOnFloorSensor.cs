using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using UnityEngine;

namespace Demos.Complex.Sensors.World
{
    public class ItemOnFloorSensor : GlobalWorldSensorBase
    {
        private ItemCollection collection;

        public override void Created()
        {
            this.collection = Object.FindObjectOfType<ItemCollection>();
        }

        public override SenseValue Sense()
        {
            return this.collection.Count(false, false);
        }
    }
}