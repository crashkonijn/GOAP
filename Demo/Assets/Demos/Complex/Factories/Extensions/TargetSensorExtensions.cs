using CrashKonijn.Goap.Classes.Builders;
using Demos.Complex.Classes;
using Demos.Complex.Interfaces;
using Demos.Complex.Sensors.Target;
using Demos.Complex.Targets;
using Demos.Shared.Sensors.Target;
using Demos.Simple.Sensors.Target;
using UnityEngine;

namespace Demos.Complex.Factories.Extensions
{
    public static class TargetSensorExtensions
    {
        public static void AddWanderTargetSensor(this GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();
        }
        
        public static void AddTransformTargetSensor(this GoapSetBuilder builder)
        {
            builder.AddTargetSensor<TransformSensor>()
                .SetTarget<TransformTarget>();
        }
        
        public static void AddClosestItemTargetSensor<T>(this GoapSetBuilder builder)
            where T : class, IHoldable
        {
            builder.AddTargetSensor<ClosestItemSensor<T>>()
                .SetTarget<ClosestTarget<T>>();
        }
        
        public static void AddClosestObjectTargetSensor<T>(this GoapSetBuilder builder)
            where T : MonoBehaviour
        {
            builder.AddTargetSensor<ClosestObjectSensor<T>>()
                .SetTarget<ClosestTarget<T>>();
        }

        public static void AddClosestSourceTargetSensor<T>(this GoapSetBuilder builder)
            where T : IGatherable
        {
            builder.AddTargetSensor<ClosestSourceSensor<T>>()
                .SetTarget<ClosestSourceTarget<T>>();
        }
    }
}