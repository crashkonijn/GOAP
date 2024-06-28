using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Core.Enums;
using CrashKonijn.Goap.Demos.Complex.Goals;
using CrashKonijn.Goap.Demos.Complex.Interfaces;
using CrashKonijn.Goap.Demos.Complex.WorldKeys;

namespace CrashKonijn.Goap.Demos.Complex.Factories.Extensions
{
    public static class GoalExtensions
    {
        public static void AddCreateItemGoal<T>(this CapabilityBuilder builder)
            where T : ICreatable
        {
            builder.AddGoal<CreateItemGoal<T>>()
                .AddCondition<CreatedItem<T>>(Comparison.GreaterThanOrEqual, 999);
        }
        
        public static void AddGatherItemGoal<T>(this CapabilityBuilder builder)
            where T : IGatherable
        {
            builder.AddGoal<GatherItemGoal<T>>()
                .AddCondition<IsInWorld<T>>(Comparison.GreaterThanOrEqual, 999);
        }
        
        public static void AddPickupItemGoal<T>(this CapabilityBuilder builder)
            where T : IHoldable
        {
            builder.AddGoal<PickupItemGoal<T>>()
                .AddCondition<IsHolding<T>>(Comparison.GreaterThanOrEqual, 1);
        }
    }
}