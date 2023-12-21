using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Demos.Complex.Sensors.Target;
using CrashKonijn.Goap.Demos.Complex.Targets;
using UnityEngine;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Extensions
{
    public static class TargetSensorExtensions
    {
        public static void AddWanderTargetSensor(this AgentTypeBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget<WanderTarget>();
        }
        
        public static void AddTransformTargetSensor(this AgentTypeBuilder builder)
        {
            builder.AddTargetSensor<TransformSensor>()
                .SetTarget<TransformTarget>();
        }
        
        public static void AddClosestItemTargetSensor<T>(this AgentTypeBuilder builder)
            where T : class, IHoldable
        {
            builder.AddTargetSensor<ClosestItemSensor<T>>()
                .SetTarget<ClosestTarget<T>>();
        }
        
        public static void AddClosestObjectTargetSensor<T>(this AgentTypeBuilder builder)
            where T : MonoBehaviour
        {
            builder.AddTargetSensor<ClosestObjectSensor<T>>()
                .SetTarget<ClosestTarget<T>>();
        }

        public static void AddClosestSourceTargetSensor<T>(this AgentTypeBuilder builder)
            where T : IGatherable
        {
            builder.AddTargetSensor<ClosestSourceSensor<T>>()
                .SetTarget<ClosestSourceTarget<T>>();
        }
    }
}