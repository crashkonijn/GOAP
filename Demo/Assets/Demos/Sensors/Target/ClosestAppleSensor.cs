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
        private AppleCollection apples;

        public ClosestAppleSensor()
        {
            this.apples = GameObject.FindObjectOfType<AppleCollection>();
        }

        public override void Update()
        {
            
        }

        public override ITarget Sense(IMonoAgent agent)
        {
            var closestApple = this.apples.Get().Closest(agent.transform.position);

            if (closestApple is null)
                return null;
            
            return new TransformTarget(closestApple.transform);
        }
    }
}