using CrashKonijn.Goap.Classes.Builders;
using Demos.Complex.Interfaces;
using Demos.Complex.Sensors.World;
using Demos.Complex.WorldKeys;

namespace Demos.Complex.Factories.Extensions
{
    public static class WorldSensorExtensions
    {
        public static void AddIsHoldingSensor<THoldable>(this AgentTypeBuilder builder)
            where THoldable : IHoldable
        {
            builder.AddWorldSensor<IsHoldingSensor<THoldable>>()
                .SetKey<IsHolding<THoldable>>();
        }
        
        public static void AddIsInWorldSensor<THoldable>(this AgentTypeBuilder builder)
            where THoldable : IHoldable
        {
            builder.AddWorldSensor<IsInWorldSensor<THoldable>>()
                .SetKey<IsInWorld<THoldable>>();
        }
        
        public static void AddItemOnFloorSensor(this AgentTypeBuilder builder)
        {
            builder.AddWorldSensor<ItemOnFloorSensor>()
                .SetKey<ItemsOnFloor>();
        }
    }
}