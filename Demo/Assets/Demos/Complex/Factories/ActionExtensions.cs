using System;
using System.Collections.Generic;
using System.Reflection;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Resolver;
using Demos.Complex.Actions;
using Demos.Complex.Behaviours;
using Demos.Complex.Classes;
using Demos.Complex.Classes.Items;
using Demos.Complex.Interfaces;
using Demos.Shared.Actions;

namespace Demos.Complex.Factories
{
    public static class ActionExtensions
    {
        public static void AddWanderAction(this GoapSetBuilder builder)
        {
            builder.AddAction<WanderAction>()
                .SetTarget(Targets.WanderTarget)
                .AddEffect(WorldKeys.IsWandering, true);
        }
        
        public static void AddPickupItemAction<T>(this GoapSetBuilder builder)
            where T : IHoldable
        {
            builder.AddAction<PickupItemAction<T>>()
                .SetTarget<T>(Targets.ClosestTarget)
                .AddEffect<T>(WorldKeys.IsHolding, true)
                .AddCondition<T>(WorldKeys.IsInWorld, Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddGatherItemAction<T>(this GoapSetBuilder builder)
            where T : ItemBase, IHoldable
        {
            builder.AddAction<GatherItem<T>>()
                .SetTarget<T>(Targets.ClosestSourceTarget)
                .AddEffect<T>(WorldKeys.IsInWorld, true);
        }
        
        public static void AddCreateItemAction<T>(this GoapSetBuilder builder)
            where T : ICreatable
        {
            var action = builder.AddAction<CreateItemAction<T>>()
                .SetTarget(Targets.TransformTarget)
                .AddEffect<T>(WorldKeys.CreatedItem, true);
            
            if (typeof(T) == typeof(Pickaxe))
            {
                action
                    .AddCondition<Iron>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 1)
                    .AddCondition<Wood>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 2);
                return;
            }
            
            if (typeof(T) == typeof(Axe))
            {
                action
                    .AddCondition<Iron>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 2)
                    .AddCondition<Wood>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 1);
                return;
            }

            throw new Exception("No conditions set for this type of item!");
        }
        
        public static void AddHaulItemAction(this GoapSetBuilder builder)
        {
            builder.AddAction<HaulItemAction>()
                .SetTarget(Targets.TransformTarget)
                .AddEffect(WorldKeys.ItemsOnFloor, false)
                .AddCondition(WorldKeys.ItemsOnFloor, Comparison.GreaterThanOrEqual, 1);
        }
        
        public static void AddEatAction(this GoapSetBuilder builder)
        {
            builder.AddAction<EatAction>()
                .SetTarget(Targets.TransformTarget)
                .AddEffect(WorldKeys.IsHungry, false)
                .AddCondition<IEatable>(WorldKeys.IsHolding, Comparison.GreaterThanOrEqual, 1);
        }
    }
}