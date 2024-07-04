using CrashKonijn.Goap.Core;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.World
{
    [GoapId("Simple-ThereAreApplesSensor")]
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