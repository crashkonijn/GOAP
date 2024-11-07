using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Sensors.Target;
using CrashKonijn.Goap.Demos.Complex.Targets;
using CrashKonijn.Goap.Runtime;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Extensions
{
    public static class TargetSensorExtensions
    {
        public static void AddClosestObjectTargetSensor<T>(this CapabilityBuilder builder)
            where T : MonoBehaviour
        {
            builder.AddTargetSensor<ClosestObjectSensor<T>>()
                .SetTarget<ClosestTarget<T>>();
        }

        public static void AddClosestSourceTargetSensor<T>(this CapabilityBuilder builder)
            where T : IGatherable
        {
            builder.AddTargetSensor<ClosestSourceSensor<T>>()
                .SetTarget<ClosestSourceTarget<T>>();
        }
    }
}
