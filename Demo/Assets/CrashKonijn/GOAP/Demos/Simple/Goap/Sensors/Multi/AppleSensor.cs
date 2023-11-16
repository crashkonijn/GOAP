using CrashKonijn.Goap.Attributes;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Demos.Simple.Behaviours;
using CrashKonijn.Goap.Demos.Simple.Goap.TargetKeys;
using CrashKonijn.Goap.Demos.Simple.Goap.WorldKeys;
using CrashKonijn.Goap.Sensors;
using Demos;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Simple.Goap.Sensors.Multi
{
    [GoapId("Simple-AppleSensor")]
    public class AppleSensor : MultiSensorBase
    {
        private AppleCollection apples;

        public override void Created()
        {
            this.apples = Object.FindObjectOfType<AppleCollection>();
        }

        public override void Update()
        {
            
        }

        public AppleSensor()
        {
            this.AddLocalTargetSensor<ClosestApple>((agent, references) =>
            {
                var closestApple = this.apples.Get().Closest(agent.transform.position);

                if (closestApple is null)
                    return null;
            
                return new TransformTarget(closestApple.transform);
            });
            
            this.AddLocalWorldSensor<HasApple>((agent, references) =>
            {
                var inventory = references.GetCachedComponent<InventoryBehaviour>();

                if (inventory == null)
                    return false;

                return inventory.Apples.Count > 0;
            });
        }
    }
}