using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Sensors;
using Demos.Simple.Behaviours;
using UnityEngine;

namespace Demos.Simple.Sensors.World
{
    public class ThereAreApplesSensor : GlobalWorldSensorBase
    {
        private readonly AppleCollection apples;

        public ThereAreApplesSensor()
        {
            this.apples = GameObject.FindObjectOfType<AppleCollection>();
        }
        
        public override SenseValue Sense()
        {
            return this.apples.Any();
        }
    }
}