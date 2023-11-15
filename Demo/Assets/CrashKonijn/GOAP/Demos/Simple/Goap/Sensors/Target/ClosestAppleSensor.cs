using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;
using CrashKonijn.Goap.Sensors;
using Demos;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Target
{
    [GoapId("Simple-ClosestAppleSensor")]
    public class ClosestAppleSensor : LocalTargetSensorBase
    {
        private AppleCollection apples;

        public override void Created()
        {
            this.apples = Object.FindObjectOfType<AppleCollection>();
        }

        public override void Update()
        {
        }

        public override ITarget Sense(IMonoAgent agent, IComponentReference references)
        {
            var closestApple = this.apples.Get().Closest(agent.transform.position);

            if (closestApple is null)
                return null;
            
            return new TransformTarget(closestApple.transform);
        }
    }
}