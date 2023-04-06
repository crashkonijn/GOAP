﻿using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Sensors;
using Demos.Complex.Behaviours;
using Demos.Complex.Interfaces;
using UnityEngine;

namespace Demos.Complex.Sensors.World
{
    public class IsInWorldSensor<T> : GlobalWorldSensorBase where T : IHoldable
    {
        private readonly ItemCollection collection;

        public IsInWorldSensor()
        {
            this.collection = GameObject.FindObjectOfType<ItemCollection>();
        }
        
        public override SenseValue Sense()
        {
            return this.collection.Get<T>().Length;
        }
    }
}