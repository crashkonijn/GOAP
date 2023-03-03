using System.Linq;
using CrashKonijn.Goap.Sensors;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.World
{
    public class ThereAreApplesSensor : GlobalWorldSensorBase
    {
        public override bool Sense()
        {
            var allApples = GameObject.FindObjectsOfType<AppleBehaviour>();
            var notPickedUpApples = allApples.Where(x => x.GetComponentInChildren<SpriteRenderer>().enabled).ToArray();

            return notPickedUpApples.Any();
        }
    }
}