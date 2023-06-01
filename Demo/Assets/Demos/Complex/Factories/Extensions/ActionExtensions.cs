using System;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Resolver;
using Demos.Complex.Actions;
using Demos.Complex.Behaviours;
using Demos.Complex.Classes.Items;
using Demos.Complex.Classes.Sources;
using Demos.Complex.Interfaces;
using Demos.Complex.Targets;
using Demos.Complex.WorldKeys;
using Demos.Shared.Actions;

namespace Demos.Complex.Factories.Extensions
{
    public static class ActionExtensions
    {
        public static void AddWanderAction(this GoapSetBuilder builder)
        {
            builder.AddAction<WanderAction>()
                .SetTarget<WanderTarget>()
                .AddEffect<IsWandering>(true);
        }
        
        public static void AddPickupItemAction<T>(this GoapSetBuilder builder)
            where T : class, IHoldable
        {
            builder.AddAction<PickupItemAction<T>>()
                .SetTarget<ClosestTarget<T>>()
                .AddEffect<IsHolding<T>>(true)
                .AddCondition<IsInWorld<T>>(Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddGatherItemAction<TGatherable, TRequired>(this GoapSetBuilder builder)
            where TGatherable : ItemBase, IGatherable
            where TRequired : IHoldable
        {
            builder.AddAction<GatherItemAction<TGatherable>>()
                .SetTarget<ClosestSourceTarget<TGatherable>>()
                .AddEffect<IsInWorld<TGatherable>>(true)
                .AddCondition<IsHolding<TRequired>>(Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddGatherItemSlowAction<TGatherable>(this GoapSetBuilder builder)
            where TGatherable : ItemBase, IGatherable
        {
            builder.AddAction<GatherItemAction<TGatherable>>()
                .SetTarget<ClosestSourceTarget<TGatherable>>()
                .AddEffect<IsInWorld<TGatherable>>(true)
                .SetBaseCost(3);
        }
        
        public static void AddCreateItemAction<T>(this GoapSetBuilder builder)
            where T : ItemBase, ICreatable
        {
            var action = builder.AddAction<CreateItemAction<T>>()
                .SetTarget<ClosestTarget<AnvilSource>>()
                .AddEffect<CreatedItem<T>>(true);
            
            if (typeof(T) == typeof(Axe))
            {
                action
                    .AddCondition<IsHolding<Iron>>(Comparison.GreaterThanOrEqual, 1)
                    .AddCondition<IsHolding<Wood>>(Comparison.GreaterThanOrEqual, 2);
                return;
            }
            
            if (typeof(T) == typeof(Pickaxe))
            {
                action
                    .AddCondition<IsHolding<Iron>>(Comparison.GreaterThanOrEqual, 2)
                    .AddCondition<IsHolding<Wood>>(Comparison.GreaterThanOrEqual, 1);
                return;
            }

            throw new Exception("No conditions set for this type of item!");
        }
        
        public static void AddHaulItemAction(this GoapSetBuilder builder)
        {
            builder.AddAction<HaulItemAction>()
                .SetTarget<TransformTarget>()
                .AddEffect<ItemsOnFloor>(false)
                .AddCondition<ItemsOnFloor>(Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddEatAction(this GoapSetBuilder builder)
        {
            builder.AddAction<EatAction>()
                .SetTarget<TransformTarget>()
                .AddEffect<IsHungry>(false)
                .AddCondition<IsHolding<IEatable>>(Comparison.GreaterThanOrEqual, 1);
        }
    }
}