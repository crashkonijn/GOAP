using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Sensors;
using Demos.Simple.Behaviours;
using UnityEngine;

namespace Demos.Simple.Sensors.World
{
    public class ThereAreApplesSensor : GlobalWorldSensorBase
    {
        private AppleCollection apples;
        
        public override void Created()
        {
            this.apples = Object.FindObjectOfType<AppleCollection>();
        }

        public override SenseValue Sense()
        {
            return this.apples.Any();
        }
    }
}