using CrashKonijn.Goap.Classes.Builders;
using Demos.Complex.Classes;
using Demos.Complex.Interfaces;
using Demos.Complex.Sensors.World;

namespace Demos.Complex.Factories.Extensions
{
    public static class WorldSensorExtensions
    {
        public static void AddIsHoldingSensor<THoldable>(this GoapSetBuilder builder)
            where THoldable : IHoldable
        {
            builder.AddWorldSensor<IsHoldingSensor<THoldable>>()
                .SetKey<THoldable>(WorldKeys.IsHolding);
        }
        
        public static void AddIsInWorldSensor<THoldable>(this GoapSetBuilder builder)
            where THoldable : IHoldable
        {
            builder.AddWorldSensor<IsInWorldSensor<THoldable>>()
                .SetKey<THoldable>(WorldKeys.IsInWorld);
        }
        
        public static void AddItemOnFloorSensor(this GoapSetBuilder builder)
        {
            builder.AddWorldSensor<ItemOnFloorSensor>()
                .SetKey(WorldKeys.ItemsOnFloor);
        }
    }
}