using CrashKonijn.Goap.Classes.Builders;
using Demos.Complex.Classes;
using Demos.Complex.Interfaces;
using Demos.Complex.Sensors.Target;
using Demos.Shared.Sensors.Target;
using Demos.Simple.Sensors.Target;

namespace Demos.Complex.Factories
{
    public static class TargetSensorExtensions
    {
        public static void AddWanderTargetSensor(this GoapSetBuilder builder)
        {
            builder.AddTargetSensor<WanderTargetSensor>()
                .SetTarget(Targets.WanderTarget);
        }
        
        public static void AddTransformTargetSensor(this GoapSetBuilder builder)
        {
            builder.AddTargetSensor<TransformSensor>()
                .SetTarget(Targets.TransformTarget);
        }
        
        public static void AddClosestItemTargetSensor<T>(this GoapSetBuilder builder)
            where T : IHoldable
        {
            builder.AddTargetSensor<ClosestItemSensor<T>>()
                .SetTarget<T>(Targets.ClosestTarget);
        }

        public static void AddClosestSourceTargetSensor<T>(this GoapSetBuilder builder)
            where T : IGatherable
        {
            builder.AddTargetSensor<ClosestSourceSensor<T>>()
                .SetTarget<T>(Targets.ClosestSourceTarget);
        }
    }
}