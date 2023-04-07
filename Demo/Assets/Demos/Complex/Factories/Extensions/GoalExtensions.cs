using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Resolver;
using Demos.Complex.Classes;
using Demos.Complex.Goals;
using Demos.Complex.Interfaces;
using Demos.Shared.Goals;

namespace Demos.Complex.Factories.Extensions
{
    public static class GoalExtensions
    {
        public static void AddWanderGoal(this GoapSetBuilder builder)
        {
            builder.AddGoal<WanderGoal>()
                .AddCondition(WorldKeys.IsWandering, Comparison.GreaterThanOrEqual, 1);
        }

        public static void AddCreateItemGoal<T>(this GoapSetBuilder builder)
            where T : ICreatable
        {
            builder.AddGoal<CreateItemGoal<T>>()
                .AddCondition<T>(WorldKeys.CreatedItem, Comparison.GreaterThanOrEqual, 1);
        }

        public static void AddCleanItemsGoal(this GoapSetBuilder builder)
        {
            builder.AddGoal<CleanItemsGoal>()
                .AddCondition(WorldKeys.ItemsOnFloor, Comparison.SmallerThanOrEqual, 0);
        }

        public static void AddFixHungerGoal(this GoapSetBuilder builder)
        {
            builder.AddGoal<FixHungerGoal>()
                .AddCondition(WorldKeys.IsHungry, Comparison.SmallerThanOrEqual, 0);
        }
        
        public static void AddGatherItemGoal<T>(this GoapSetBuilder builder)
            where T : IGatherable
        {
            builder.AddGoal<GatherItemGoal<T>>()
                .AddCondition<T>(WorldKeys.IsInWorld, Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddPickupItemGoal<T>(this GoapSetBuilder builder)
            where T : IHoldable
        {
            builder.AddGoal<PickupItemGoal<T>>()
                .AddCondition<T>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 1);
        }
    }
}