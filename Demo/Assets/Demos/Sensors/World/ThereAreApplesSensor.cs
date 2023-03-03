using CrashKonijn.Goap.Sensors;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.World
{
    public class ThereAreApplesSensor : GlobalWorldSensorBase
    {
        private readonly AppleCollection apples;

        public ThereAreApplesSensor()
        {
            this.apples = GameObject.FindObjectOfType<AppleCollection>();
        }
        
        public override bool Sense()
        {
            return this.apples.Any();
        }
    }
}