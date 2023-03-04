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
        
        public override bool Sense()
        {
            return this.apples.Any();
        }
    }
}