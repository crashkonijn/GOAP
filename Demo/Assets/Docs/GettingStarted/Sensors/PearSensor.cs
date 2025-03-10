using System.Collections.Generic;
using CrashKonijn.Docs.GettingStarted.Behaviours;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Docs.GettingStarted.Sensors
{
    // Defining a GoapId is only necessary when using the ScriptableObject configuration method.
    [GoapId("PearSensor-d68c875d-29c0-43f3-9d79-054d4cc6505d")]
    public class PearSensor : MultiSensorBase
    {
        // A cache of all the pears in the world
        private PearBehaviour[] pears;

        // You must use the constructor to register all the sensors
        // This can also be called outside of the gameplay loop to validate the configuration
        public PearSensor()
        {
            this.AddLocalWorldSensor<PearCount>((agent, references) =>
            {
                // Get a cached reference to the DataBehaviour on the agent
                var data = references.GetCachedComponent<DataBehaviour>();

                return data.pearCount;
            });

            this.AddLocalWorldSensor<Hunger>((agent, references) =>
            {
                // Get a cached reference to the DataBehaviour on the agent
                var data = references.GetCachedComponent<DataBehaviour>();

                // We need to cast the float to an int, because the hunger is an int
                // We will lose the decimal values, but we don't need them for this example
                return (int) data.hunger;
            });

            this.AddLocalTargetSensor<ClosestPear>((agent, references, target) =>
            {
                // Use the cashed pears list to find the closest pear
                var closestPear = this.Closest(this.pears, agent.Transform.position);

                if (closestPear == null)
                    return null;

                // If the target is a transform target, set the target to the closest pear
                if (target is TransformTarget transformTarget)
                    return transformTarget.SetTransform(closestPear.transform);

                return new TransformTarget(closestPear.transform);
            });
        }

        // The Created method is called when the sensor is created
        // This can be used to gather references to objects in the scene
        public override void Created() { }

        // This method is equal to the Update method of a local sensor.
        // It can be used to cache data, like gathering a list of all pears in the scene.
        public override void Update()
        {
            this.pears = Object.FindObjectsOfType<PearBehaviour>();
        }

        // Returns the closest item in a list
        private T Closest<T>(IEnumerable<T> list, Vector3 position)
            where T : MonoBehaviour
        {
            T closest = null;
            var closestDistance = float.MaxValue; // Start with the largest possible distance

            foreach (var item in list)
            {
                var distance = Vector3.Distance(item.gameObject.transform.position, position);

                if (!(distance < closestDistance))
                    continue;

                closest = item;
                closestDistance = distance;
            }

            return closest;
        }
    }
}
