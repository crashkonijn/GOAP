using System.Linq;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using Demos.Behaviours;
using UnityEngine;

namespace Demos.Sensors.Target
{
    public class ClosestAppleSensor : LocalTargetSensorBase
    {
        public override ITarget Sense(IMonoAgent agent)
        {
            var allApples = GameObject.FindObjectsOfType<AppleBehaviour>();
            var notPickedUpApples = allApples.Where(x => x.GetComponentInChildren<SpriteRenderer>().enabled).ToArray();

            var closestApple = notPickedUpApples.Closest(agent.transform.position);

            if (closestApple is null)
                return null;
            
            return new TransformTarget(closestApple.transform);
        }
    }
}